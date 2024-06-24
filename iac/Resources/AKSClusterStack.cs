using System;
using System.Collections.Generic;
using System.Text;
using Pulumi;
using Pulumi.Kubernetes;
using Pulumi.AzureNative.ContainerService;
using iac_az_ia_ollama.Resources.Helm.Charts;

namespace iac_az_ia_ollama.Resources;

public class AKSClusterStack
{
    public AKSClusterStack(Pulumi.AzureNative.Resources.ResourceGroup resourceGroup)
    {
        var config = new Pulumi.Config();

        var aksClusterStack = new ManagedCluster(config.Require("AKSCluster.Name"), new()
        {
            ResourceGroupName = resourceGroup.Name,
            ResourceName = config.Require("AKSCluster.Name"),
            AgentPoolProfiles = new[]
            {
                new Pulumi.AzureNative.ContainerService.Inputs.ManagedClusterAgentPoolProfileArgs
                {
                    Name = config.Require("AKSCluster.AgentPool.Name"),
                    Count = config.RequireInt32("AKSCluster.AgentPool.Count"),
                    MinCount = config.RequireInt32("AKSCluster.AgentPool.MinCount"),
                    MaxCount = config.RequireInt32("AKSCluster.AgentPool.MaxCount"),
                    EnableAutoScaling = true,
                    VmSize = config.Require("AKSCluster.AgentPool.VmSize"),
                    Mode = AgentPoolMode.System,
                }
            },

            Sku = new Pulumi.AzureNative.ContainerService.Inputs.ManagedClusterSKUArgs
            {
                Name = "Base",
                Tier = config.Require("AKSCluster.SKUTier")
            },

            ApiServerAccessProfile = new Pulumi.AzureNative.ContainerService.Inputs.ManagedClusterAPIServerAccessProfileArgs
            {
                AuthorizedIPRanges = config.RequireObject<List<string>>("AKSCluster.AuthorizedIPRanges")
            },

            DnsPrefix = config.Require("AKSCluster.DnsPrefix"),
            NetworkProfile = new Pulumi.AzureNative.ContainerService.Inputs.ContainerServiceNetworkProfileArgs
            {
                NetworkPolicy = "Calico",
            },

            Identity = new Pulumi.AzureNative.ContainerService.Inputs.ManagedClusterIdentityArgs
            {
                Type = ResourceIdentityType.SystemAssigned
            },
            
        },

        new CustomResourceOptions()
        {
            DependsOn = new Resource[] { resourceGroup }
        });

        var creds = Output.Tuple(resourceGroup.Name, aksClusterStack.Name).Apply(async t =>
        {
            var credentials = await ListManagedClusterUserCredentials.InvokeAsync(new ListManagedClusterUserCredentialsArgs
            {
                ResourceGroupName = t.Item1,
                ResourceName = t.Item2,
            });

            var kubeconfig = Encoding.UTF8.GetString(Convert.FromBase64String(credentials.Kubeconfigs[0].Value));
            return kubeconfig;
        });

        var k8sProvider = new Provider("k8s-provider", new ProviderArgs
        {
            KubeConfig = creds
        }, new CustomResourceOptions
        {
            DependsOn = { aksClusterStack }
        });

        var nginxIngressController = new IngressNginx(aksClusterStack, k8sProvider);
        var openWebUIChart = new OpenWebUI(aksClusterStack, k8sProvider);
    }
}

using Pulumi.Kubernetes;
using Pulumi.AzureNative.ContainerService;
using Pulumi.Kubernetes.Helm.V3;
using Pulumi.Kubernetes.Helm;
using Pulumi;

namespace iac_az_ia_ollama.Resources.Helm.Charts;

public class IngressNginx
{
    public Chart chartNginxIngressController { get; }

    public IngressNginx(ManagedCluster cluster, Provider provider)
    {
        var config = new Pulumi.Config();

        var nginxIngressController = new Chart("ingress-nginx", new ChartArgs()
        {
            Chart = "ingress-nginx",
            Version = "4.9.1",
            FetchOptions = new ChartFetchArgs
            {
                Repo = "https://kubernetes.github.io/ingress-nginx",
            },
        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = new[] { cluster }
        });

        this.chartNginxIngressController = nginxIngressController;
    }
}

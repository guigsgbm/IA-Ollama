using Pulumi;
using Pulumi.AzureNative.ContainerService;
using Pulumi.Kubernetes;
using Pulumi.Kubernetes.Helm;
using Pulumi.Kubernetes.Helm.V3;
using System.Collections.Generic;

namespace iac_az_ia_ollama.Resources.Helm.Charts;

public class OpenWebUI
{
    public OpenWebUI(ManagedCluster cluster, Provider provider)
    {
        var config = new Pulumi.Config();

        var OpenWebUI = new Chart("open-webui", new ChartArgs()
        {
            Chart = "open-webui",
            Version = "3.0.4",
            FetchOptions = new ChartFetchArgs
            {
                Repo = "https://helm.openwebui.com/",
            },

            Values = new Dictionary<string, object>
            {
                {"ingress", new Dictionary<string, object>
                {
                    {"enabled", true },
                    {"class", "nginx" },
                    {"annotations", new Dictionary<string, object>
                        {
                            { "nginx.ingress.kubernetes.io/rewrite-target", "/" }
                        }
                    }
                } },

                {"pipelines", new Dictionary<string, object>
                {
                    {"enabled", false },
                } }
            }

        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = new[] { cluster }
        });
    }
}

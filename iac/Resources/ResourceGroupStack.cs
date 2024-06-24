using Pulumi.AzureNative.Resources;

namespace iac_az_ia_ollama.Resources;

public class ResourceGroupStack
{
    public ResourceGroup resourceGroup { get; }

    public ResourceGroupStack()
    {
        var config = new Pulumi.Config();

        var resourceGroupResource = new ResourceGroup(config.Require("ResourceGroup.Name"), new()
        {
            ResourceGroupName = config.Require("ResourceGroup.Name"),
        });

        this.resourceGroup = resourceGroupResource;
    }
}

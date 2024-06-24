using iac_az_ia_ollama.Resources;

return await Pulumi.Deployment.RunAsync(() =>
{
    var rg = new ResourceGroupStack();
    var aks = new AKSClusterStack(rg.resourceGroup);
});
# IA-Ollama

### Necessario:

- Realizar login no Azure e no Pulumi.

### Comandos:
```
git clone https://github.com/guigsgbm/IA-Ollama.git

cd .\iac\

pulumi up -y
```

### Aplica��o:
```
az aks get-credentials --resource-group rg_ia_ollama --name aks_ia_ollama --overwrite-existing

kubectl get svc
```
![k8s services](Images/services.png)

- O app estara disponivel no EXTERNAL-IP.
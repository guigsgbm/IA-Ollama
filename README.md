# IA-Ollama

### Necessario:

- Realizar login no Azure e no Pulumi.

### Comandos:
```
git clone https://github.com/guigsgbm/IA-Ollama.git

cd .\iac\

pulumi up -y
```

### Aplicação:
```
az aks get-credentials --resource-group rg_ia_ollama --name aks_ia_ollama --overwrite-existing

kubectl get svc
```
![Logo do Projeto](images/services.png)

- O app estara disponivel no EXTERNAL-IP.
#!/bin/bash
kubectl create -f messagebus/deployment.yaml
kubectl create -f messagebus/service.yaml

kubectl create -f eventstore/deployment.yaml
kubectl create -f eventstore/service.yaml

kubectl create -f query/deployment.yaml
kubectl create -f query/service.yaml

kubectl create secret generic command-secret-appsettings --from-file=command/appsettings.secrets.json
kubectl create -f command/deployment.yaml
kubectl create -f command/service.yaml

kubectl create configmap nginx-frontend-conf --from-file=frontend/nginx.conf
kubectl create -f frontend/deployment.yaml
kubectl create -f frontend/service.yaml

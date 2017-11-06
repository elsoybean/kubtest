#!/bin/bash
kubectl create -f query/deployment.yaml
kubectl create -f query/service.yaml

kubectl create configmap nginx-frontend-conf --from-file=frontend/nginx.conf
kubectl create -f frontend/deployment.yaml
kubectl create -f frontend/service.yaml

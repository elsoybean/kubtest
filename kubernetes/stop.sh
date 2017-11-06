#!/bin/bash
kubectl delete service query command frontend
kubectl delete deployment query command frontend
kubectl delete configmap nginx-frontend-conf
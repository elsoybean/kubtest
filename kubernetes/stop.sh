#!/bin/bash
kubectl delete deployment query frontend
kubectl delete service query frontend
kubectl delete configmap nginx-frontend-conf
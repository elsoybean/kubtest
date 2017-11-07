#!/bin/bash
kubectl delete service query command messagebus frontend
kubectl delete deployment query command messagebus frontend
kubectl delete configmap nginx-frontend-conf
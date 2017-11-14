#!/bin/bash
kubectl delete service query command messagebus eventstore frontend ecdb
kubectl delete deployment query command messagebus frontend
kubectl delete configmap nginx-frontend-conf ecdb ecdb-init
kubectl delete secret command-secret-appsettings
kubectl delete statefulsets eventstore ecdb

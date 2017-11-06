# Introduction
This repository is a collection of source and scripts to orchestrate several independently scalable microservices with Kubernetes on the Google cloud.

## Credit
I learned most of this from an [excellent overview](https://codelabs.developers.google.com/codelabs/cloud-orchestrate-with-kubernetes/index.html) of orchestrating Kubernetes services on Google Cloud.

## Before You Start
Before you can get started with Kubernetes commands, you need to hook up a cloud provider. I'm using Google Cloud Platform, but I think everything after this section will work with anything that supports Kubernetes.
- Create a Google account if you don't have one.
- Sign up for the free trial of [Google Cloud Platform](https://cloud.google.com/). You get a year and $300 free to try it out, but you may have to enable billing so they know you are real.
- Create a project on GCP
- Ensure you have a reasonable terminal to work in. I like bash, so Git bash works for me.
- Download and install the [Google Cloud SDK](https://cloud.google.com/sdk/docs/quickstart-windows) and go through the initialization so you can run gcloud commands in your terminal. Make sure you pick a compute zone. I used us-east4-a.
- Create a cluster with `gcloud container clusters create --num-nodes=6 <cluster name>`
Now you have a cluster with 6 nodes ready to run Kubernetes pods to support services.

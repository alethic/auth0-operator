{{/*
Expand the name of the chart.
*/}}
{{- define "auth0-operator.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "auth0-operator.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "auth0-operator.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "auth0-operator.selectorLabels" -}}
app.kubernetes.io/name: {{ include "auth0-operator.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "auth0-operator.labels" -}}
helm.sh/chart: {{ include "auth0-operator.chart" . }}
{{ include "auth0-operator.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- with .Values.operator.additionalLabels }}
{{- toYaml . }}
{{- end }}
{{- end }}

{{/*
Generate a common CA certificate
We store this certificate in .Values.global.CA*
*/}}
{{- define "auth0-operator.gen-ca-cert" -}}
{{- if not .Values.global.CA }}
  {{- $ca := genCA "auth0-operator-ca" 365 }}
  {{- $_ := set .Values.global "CA" $ca }}
  {{- $_ := set .Values.global "CACertificate" ($ca.Cert | b64enc) }}
  {{- $_ := set .Values.global "CAKey" ($ca.Key | b64enc) }}
{{- end }}
{{- end }}

{{/*
Generate a common webhook certificate
We store this certificate in .Values.global.Webhook*
*/}}
{{- define "auth0-operator.gen-webhook-cert" -}}
{{- template "auth0-operator.gen-ca-cert" . }}
{{- if not .Values.global.Webhook }}
  {{- $cert := genSignedCert (include "auth0-operator.fullname" .) nil (list (printf "%s.%2s.svc" (include "auth0-operator.fullname" .) .Release.Namespace) (printf "%s.%s.svc.%s" (include "auth0-operator.fullname" .) .Release.Namespace "cluster.local")) 365 .Values.global.CA }}
  {{- $_ := set .Values.global "Webhook" $cert }}
  {{- $_ := set .Values.global "WebhookCertificate" ($cert.Cert | b64enc) }}
  {{- $_ := set .Values.global "WebhookKey" ($cert.Key | b64enc) }}
{{- end }}
{{- end }}

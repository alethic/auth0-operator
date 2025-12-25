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
Selector labels.
*/}}
{{- define "auth0-operator.selectorLabels" -}}
app.kubernetes.io/name: {{ include "auth0-operator.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Common labels.
*/}}
{{- define "auth0-operator.labels" -}}
helm.sh/chart: {{ include "auth0-operator.chart" . }}
{{ include "auth0-operator.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- with .Values.commonLabels }}
{{- toYaml . }}
{{- end }}
{{- end }}

{{/*
Common annotations.
*/}}
{{- define "auth0-operator.annotations" -}}
{{- with .Values.commonAnnotations }}
{{- toYaml . }}
{{- end }}
{{- end }}

{{/*
Generate a common CA certificate
We store this certificate in .Values._.CA*
*/}}
{{- define "auth0-operator.gen-ca-cert" -}}
{{- if not .Values._.CA }}
  {{- $ca := genCA "auth0-operator-ca" 3650 }}
  {{- $_ := set .Values._ "CA" $ca }}
  {{- $_ := set .Values._ "CACert" ($ca.Cert | b64enc) }}
  {{- $_ := set .Values._ "CAKey" ($ca.Key | b64enc) }}
{{- end }}
{{- end }}

{{/*
Generate a common TLS certificate
We store this certificate in .Values._.TLS*
*/}}
{{- define "auth0-operator.gen-tls-cert" -}}
{{- template "auth0-operator.gen-ca-cert" . }}
{{- if not .Values._.TLS }}
  {{- $cert := genSignedCert "auth0-operator-tls" nil (list (printf "%s.%2s.svc" (include "auth0-operator.fullname" .) .Release.Namespace) (printf "%s.%s.svc.%s" (include "auth0-operator.fullname" .) .Release.Namespace .Values.clusterDomain)) 3650 .Values._.CA }}
  {{- $_ := set .Values._ "TLS" $cert }}
  {{- $_ := set .Values._ "TLSCert" ($cert.Cert | b64enc) }}
  {{- $_ := set .Values._ "TLSKey" ($cert.Key | b64enc) }}
{{- end }}
{{- end }}

{{/*
CRD labels.
*/}}
{{- define "auth0-operator.crd.labels" -}}
{{ include "auth0-operator.labels" . }}
{{- end }}

{{/*
CRD annotations.
*/}}
{{- define "auth0-operator.crd.annotations" -}}
helm.sh/resource-policy: keep
{{ include "auth0-operator.annotations" . }}
{{- end }}

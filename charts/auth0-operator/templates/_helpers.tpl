{{- /* Expand the name of the chart. */}}
{{- define "auth0-operator.name" -}}
{{-   default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- /* Create a default fully qualified app name. We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec). If release name contains chart name it will be used as a full name. */}}
{{- define "auth0-operator.fullname" -}}
{{-   if .Values.fullnameOverride }}
{{-     .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{-   else }}
{{-     $name := default .Chart.Name .Values.nameOverride }}
{{-     if contains $name .Release.Name }}
{{-       .Release.Name | trunc 63 | trimSuffix "-" }}
{{-     else }}
{{-       printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{-     end }}
{{-   end }}
{{- end }}

{{- /* Create chart name and version as used by the chart label. */}}
{{- define "auth0-operator.chart" -}}
{{-   printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{- /* Selector labels. */}}
{{- define "auth0-operator.selectorLabels" -}}
app.kubernetes.io/name: {{ include "auth0-operator.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- /* Common labels. */}}
{{- define "auth0-operator.labels" -}}
helm.sh/chart: {{ include "auth0-operator.chart" . }}
{{    include "auth0-operator.selectorLabels" . }}
{{-   if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{-   end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{-   with .Values.commonLabels }}
{{-     toYaml . }}
{{-   end }}
{{- end }}

{{- /* Common annotations. */}}
{{- define "auth0-operator.annotations" -}}
{{-   with .Values.commonAnnotations }}
{{-     toYaml . }}
{{-   end }}
{{- end }}

{{- /* Generate or retrieve self-signed certificates for the operator. */}}
{{- define "auth0-operator.selfsignedcerts" -}}
{{-   if not .Values._.selfsignedcerts }}
{{-     $secret := default (dict "data" (dict)) (lookup "v1" "Secret" "" (printf "%s-tls" ( include "auth0-operator.fullname" . ))) }}
{{-     $cacrt  := index $secret.data "ca.crt"  | default "" }}
{{-     $tlscrt := index $secret.data "tls.crt" | default "" }}
{{-     $tlskey := index $secret.data "tls.key" | default "" }}
{{-     if and $cacrt $tlscrt $tlskey }}
{{-       $_    := set .Values._ "selfsignedcerts" (dict "cacrt" $cacrt "tlscrt" $tlscrt "tlskey" $tlskey) }}
{{-     else }}
{{-       $ca   := genCA "auth0-operator-ca" 3650 }}
{{-       $tls  := genSignedCert "auth0-operator-tls" nil (list (printf "%s.%2s.svc" (include "auth0-operator.fullname" .) .Release.Namespace) (printf "%s.%s.svc.%s" (include "auth0-operator.fullname" .) .Release.Namespace (.Values.clusterDomain | default "cluster.local"))) 3650 $ca }}
{{-       $_    := set .Values._ "selfsignedcerts" (dict "cacrt" ($ca.Cert | b64enc) "tlscrt" ($tls.Cert | b64enc) "tlskey" ($tls.Key | b64enc)) }}
{{-     end }}
{{-   end }}
{{- end }}

{{- /* CRD labels. */}}
{{- define "auth0-operator.crd.labels" -}}
{{    include "auth0-operator.labels" . }}
{{- end }}

{{- /* CRD annotations. */}}
{{- define "auth0-operator.crd.annotations" -}}
{{    include "auth0-operator.annotations" . }}
{{-   if .Values.crds.keep -}}
helm.sh/resource-policy: keep
{{-   end }}
{{- end }}

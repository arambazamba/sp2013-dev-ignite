$devSite = "http://sp2013/sites/SpAppsCloudHosted"
$appName = "Provider Hosted App"
$issuerId = [System.Guid]::NewGuid().ToString()
$certPath = "D:\Presentations & Demos\05_AppModelCloudHosted\HighTrustAppCert.cer"
$devSiteWeb = Get-SPWeb $devSite
$devSiteRealm = Get-SPAuthenticationRealm –ServiceContext $devSiteWeb.Site
$appCert = Get-PfxCertificate $certPath 
$fullAppId = $issuerId + '@' + $devSiteRealm 
$secureTokenIssuer = New-SPTrustedSecurityTokenIssuer –Name $appName –Certificate $appCert –RegisteredIssuerName $fullAppId 
$appPrincipal = Register-SPAppPrincipal –NameIdentifier $fullAppId –Site $devSiteWeb –DisplayName $appName 
$stsConfig = Get-SPSecurityTokenServiceConfig
$stsConfig.AllowOAuthOverHttp = $true;
$stsConfig.Update()
$issuerId 
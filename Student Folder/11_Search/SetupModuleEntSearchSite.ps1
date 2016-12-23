# =+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
# README: Change variables in lines 7-11 for each module
# =+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=

Write-Host 

$rootWebApp = "http://intranet.contoso.com"
$SiteCollectionOwner = "contoso\Administrator"
$SiteTitle = "Ignite - Enterprise Search Center"
$SiteUrl = "/sites/Search"
$SiteTemplate = "SRCHCEN#0"

# load Microsoft.SharePoint.PowerShell snapin
Write-Host "{Step 1 of 4} Loading SharePoint PowerShell snapin..." -ForegroundColor White
Add-PSSnapin Microsoft.SharePoint.Powershell -ErrorAction "SilentlyContinue"

# delete any existing site found at target URL
$newSiteUrl = $rootWebApp+$SiteUrl
Write-Host
Write-Host "{Step 2 of 4} Checking for existing site at" $newSiteUrl -ForegroundColor White
$targetUrl = Get-SPSite -Limit All | Where-Object {$_.Url -eq $newSiteUrl}
if ($targetUrl -ne $null) {
	Write-Host " >>> Deleting existing site collection at" $newSiteUrl -ForegroundColor DarkGray
	Remove-SPSite -Identity $newSiteUrl -Confirm:$false
}

# create new site at target URL
Write-Host
Write-Host "{Step 3 of 4} Creating new site collection at" $newSiteUrl "..." -ForegroundColor White
$NewSite = New-SPSite -URL $newSiteUrl -OwnerAlias $SiteCollectionOwner -Template $SiteTemplate -Name $SiteTitle
$RootWeb = $NewSite.RootWeb

# display site info
Write-Host 
Write-Host "Site collection created successfully!" -foregroundcolor Green
Write-Host "-------------------------------------" -foregroundcolor White	
Write-Host "URL  :" $RootWeb.Url -foregroundcolor White
Write-Host "ID   :" $RootWeb.Id.ToString() -foregroundcolor White
Write-Host "Title:" $RootWeb.Title -foregroundcolor White
Write-Host "-------------------------------------" -foregroundcolor White

# launch IE in 32-bit mode with the site
Write-Host 
Write-Host "{Step 4 of 4} Launching Internet Explorer (32-bit) with the site..." -ForegroundColor White
start iexplore $RootWeb.Url

Write-Host
Write-Host "It is safe to close this console..." -foregroundcolor Green
Write-Host
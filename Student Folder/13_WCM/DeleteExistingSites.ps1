
function DeleteExistingSite($url)
{
    
    $targetUrl = Get-SPSite -Limit All | Where-Object {$_.Url -eq $url}
    if ($targetUrl -ne $null) 
    {
	    Write-Host " >>> Deleting existing site collection at" $url -ForegroundColor DarkGray
	    Remove-SPSite -Identity $url -Confirm:$false
    }
    
}

Write-Host 

# load Microsoft.SharePoint.PowerShell snapin
Write-Host "{Step 1 of 4} Loading SharePoint PowerShell snapin..." -ForegroundColor White
Add-PSSnapin Microsoft.SharePoint.Powershell -ErrorAction "SilentlyContinue"


$url = "http://intranet.contoso.com/sites/ProductCatalog"
Write-Host "{Step 2 of 4} Checking for existing site at" $url -ForegroundColor White
DeleteExistingSite $url

$url = "http://intranet.contoso.com/sites/Catalog"
Write-Host "{Step 3 of 4} Checking for existing site at" $url -ForegroundColor White
DeleteExistingSite $url

$url = "http://intranet.contoso.com/sites/CatalogHOL"
Write-Host "{Step 4 of 4} Checking for existing site at" $url -ForegroundColor White
DeleteExistingSite $url


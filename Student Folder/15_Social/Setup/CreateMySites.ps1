write-host "RUN ME AS THE SHAREPOINT SERVICE ACCOUNT!"

[Reflection.Assembly]::LoadWithPartialName("Microsoft.Office.Server")

$mysiteHostUrl = "http://my.contoso.com"
$mysite = Get-SPSite $mysiteHostUrl

$context = [Microsoft.Office.Server.ServerContext]::GetContext($mysite)
$upm =  New-Object Microsoft.Office.Server.UserProfiles.UserProfileManager($context)

$AllProfiles = $upm.GetEnumerator()

foreach($profile in $AllProfiles)
{

    $DisplayName = $profile.DisplayName
    $AccountName = $profile[[Microsoft.Office.Server.UserProfiles.PropertyConstants]::AccountName].Value

          if($profile.PersonalSite -eq $Null)
          {
               write-host "Creating personal site for ", $AccountName
               $profile.CreatePersonalSite()
          }
          else
          {
               write-host $AccountName ," already has personal site"
          }

}
$mysite.Dispose();
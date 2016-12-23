Import-Module ac*
$pwd = ConvertTo-SecureString -AsPlainText -Force -String "pass@word1"
$users = Get-ADUSer -Filter "*" -SearchBase "OU=Employees,DC=corp,DC=contoso,DC=com"
$users | ForEach-Object {Set-ADAccountPassword -Identity $_ -Reset -NewPassword $pwd}
Import-module activedirectory

function ClearVariables($sAMAccountName, $name, $sn, $givenName, $distinguishedName, $title, $telephoneNumber, $department, $manager, $mail)
{
    $sAMAccountName = "";
    $name = "";
    $sn = "";
    $givenName = "";
    $distinguishedName = "";
    $title = "";
    $telephoneNumber = "";
    $department = "";
    $manager = "";
    $mail= "";
}




function CreateUsers($filePath)
{
    $dnsDomain =gc env:USERDNSDOMAIN
    $split = $dnsDomain.split(".")
    if ($split[2] -ne $null) 
    {
	    $domain = "DC=$($split[0]),DC=$($split[1]),DC=$($split[2])"
    } 
    else 
    {
	    $domain = "DC=$($split[0]),DC=$($split[1])"
    }

    $orgUnit = "OU=Employees"
    $password = ConvertTo-SecureString -AsPlainText "pass@word1" -Force

    $path = $orgUnit + ',' + $domain;    

    $importFile = Get-Content $filePath

    foreach($line in $importFile)
    {    
        if($line)
        {
            $key = $line.Split(":")[0].Trim();
            $value = $line.Split(":")[1].Trim();
    

            if($key -eq "sAMAccountName") { $sAMAccountName = $value;}
            if($key -eq "name") { $name = $value;}
            if($key -eq "sn") { $sn = $value;}
            if($key -eq "givenName") { $givenName = $value;}
            if($key -eq "title") { $title = $value;}
            if($key -eq "telephoneNumber") { $telephoneNumber = $value;}
            if($key -eq "department") { $department = $value;}
            if($key -eq "manager") { $manager = $value;}
            if($key -eq "mail") { $mail = $value;}      
        }
        else
        {
            New-ADUser -SamAccountName $samAccountName -Name $name -Surname $sn -GivenName $givenName -Path $path -AccountPassword $password -Enabled $true -title $title -officePhone $telephoneNumber -department $department -emailaddress $mail -PasswordNeverExpires $true                        
            Add-ADGroupMember -Identity "PortalUsers" -Member $samAccountName 
            ClearVariables $sAMAccountName $name $sn $givenName $title $telephoneNumber $department $manager $mail
        }
    }
}


function UpdateManagers($filePath)
{
    $dnsDomain =gc env:USERDNSDOMAIN
    $split = $dnsDomain.split(".")
    if ($split[2] -ne $null) 
    {
	    $domain = "DC=$($split[0]),DC=$($split[1]),DC=$($split[2])"
    } 
    else 
    {
	    $domain = "DC=$($split[0]),DC=$($split[1])"
    }

    $orgUnit = "OU=Employees"

    $path = $orgUnit + ',' + $domain;
    
    $importFile = Get-Content $filePath
    
    foreach($line in $importFile)
    {    
        if($line)
        {
            $key = $line.Split(":")[0].Trim();
            $value = $line.Split(":")[1].Trim();
    

            if($key -eq "sAMAccountName") { $sAMAccountName = $value;}            
            if($key -eq "manager") { $manager = $value;}            
        }
        else
        {
            if($manager)
            {
                Set-ADUser $samAccountName -Replace @{manager=$manager}
            }
            ClearVariables $sAMAccountName $manager 
        }
    }
}

CreateUsers "c:\temp\users.txt"
UpdateManagers "c:\temp\users.txt"
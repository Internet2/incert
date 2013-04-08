<?php
	include_once 'ua-parser/php/uaparser.php';
	
	// Parse the user agent
	$parser = new UAParser;
	$result = $parser->parse($_SERVER[HTTP_USER_AGENT]);
	
	// Grab value for later use
	$osFamily = $result->os->family;
?>

<!DOCTYPE html>

<html lang="en">

<head>
	<meta charset="utf-8" />
	<title>InCert</title>
</head>

<body>
	<h3>InCert (InCommon Certificate Provisioning Tool)</h3>

	<p>Select your site and enter your credentials to receive your iOS/OS X Configuration Profile.</p>

	<form method="post" action="generateConfig.php" id="configForm">
		<p> Site:
			<select name="template">
				<option value="iu">Indiana University</option>
				<option value="uva">University of Virginia</option>
				</select>
		</p>

		<p>
		Login-ID: <input type="text" name="LoginId" /><br />
		Password: <input type="password" name="LoginPwd" /><br />
		Last Name: <input type="text" name="LoginCred2" value="cred2" /><br />
		ID Number: <input type="text" name="LoginCred3" value="cred3" /><br />
		Date of Birth: <input type="text" name="LoginCred4" value="cred4" /><br />
		</p>

		<p> <input type="submit" /> </p>
	</form>
</body>

</html>

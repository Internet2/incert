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
	<meta name="viewport" content="width=device-width, user-scalable=no">
	<title>InCert</title>
    <link rel="stylesheet" href="css/jquery.mobile.structure-1.3.1.min.css" />
    <link rel="stylesheet" href="css/themes/jquery.mobile.theme-1.3.1.min.css" />
    <script src="js/jquery-2.0.3.min.js"></script>
    <script src="js/jquery.mobile-1.3.1.min.js"></script>
</head>

<body>

<div data-role="page">
	
	<header data-role="header">
	<a href="../" data-ajax="false" data-icon="back" data-iconpos="notext">Back</a>
    <h3>InCert</h3>
    </header>
	
    <div data-role="content">
	<p>Select your site and enter your credentials to receive your iOS/OS X Configuration Profile.</p>

	<form method="post" action="generateConfig.php" data-ajax="false">
		<label for="template">Site:</label>
		<select name="template">
			<option value="iu">Indiana University</option>
			<option value="uva">University of Virginia</option>
			<option value="incommon" selected>InCommon Test (incommontest.org)</option>
		</select>
		<label for="LoginId">Login-ID:</label>
		<input type="text" name="LoginId" data-clear-btn="true" />
		<label for="LoginPwd">Password:</label>
		<input type="password" name="LoginPwd" data-clear-btn="true" />
		<label for="LoginCred2">Last Name:</label>
		<input type="text" name="LoginCred2" value="cred2" data-clear-btn="true" />
		<label for="LoginCred3">ID Number:</label>
		<input type="text" name="LoginCred3" value="cred3" data-clear-btn="true" />
		<label for="LoginCred4">Date of Birth:</label>
		<input type="text" name="LoginCred4" value="cred4" data-clear-btn="true" />
		<input type="submit" value="Submit"/>
	</form>
    </div>
	
	<footer data-role="footer">
	<h3>© 2013 InCommon LLC</h3>
	</footer>

</div>

</body>

</html>

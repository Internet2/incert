<?php
require_once("includes/functions.php");

// turn debug on if you want to see output instead of downloading the profile

$debug = FALSE;

// define the variables using POST
$username = $_POST['LoginId'];
$password = $_POST['LoginPwd'];
$template_id = $_POST['template'];
$cred2 = $_POST['LoginCred2'];
$cred3 = $_POST['LoginCred3'];
$cred4 = $_POST['LoginCred4'];

// Find the selected template
$template_file = "templates/" . $template_id . "_template.mobileconfig";
if (!file_exists($template_file)) {
		header( 'Location: ' .  TEMPLATE_ERROR_PAGE);
		exit();
}

// Get the profile identifier from the template
$profileID = getProfileID($template_file);

// Get the cert from Incommon
$certData = getCertificate($username, $password, $cred2, $cred3, $cred4);

// Continue only if we have certificate data
if ($certData) {
	// Extract each item in the PCKS12 data
	openssl_pkcs12_read($certData, $certsArray, $password);
	
	// If there are bundled certs
	if (array_key_exists('extracerts', $certsArray)) {
		// Get the extracerts array
		$extraCertsArray = $certsArray['extracerts'];
		
		// Build the certificate payload dictionaries
		$certDictArray = array();
		foreach ($extraCertsArray as $extraCert) {
			
			// Trim the headers and footers
			$search = array( "-----BEGIN CERTIFICATE-----\n", "-----END CERTIFICATE-----\n" );
			$trimmedExtraCert = str_replace($search, "", $extraCert);
			
			// Parse the certificate
			$parsedExtraCertArray = openssl_x509_parse($extraCert);
			
			// If the cert is self-signed, make it a root payload
			// Else make it a regular PKCS1 payload
			if ($parsedExtraCertArray["subject"] === $parsedExtraCertArray["issuer"]) {
				$certDictArray[] = rootPayload($profileID, $trimmedExtraCert);
			} else {
				$certDictArray[] = pkcs1Payload($profileID, $trimmedExtraCert);
			}
			
		}
		
		// Reverse the array so that the root cert is the first element
		$certDictArray = array_reverse($certDictArray);
	}
	
	openssl_pkcs12_export($certsArray['cert'], $pkcs12Data, $certsArray['pkey'], $password);
	
	// Build the PKCS12 payload dictionary
	$pkcs12Dict = pkcs12Payload($profileID, $pkcs12Data);

	// Get the UUID from the PKCS12 payload
	$pkcs12UUID = $pkcs12Dict->get(PAYLOAD_UUID)->getValue();
	
	// Replace all occurences of $username$ and $pkcs12UUID$ in the template
	// with the actual username and pkcs12UUID
	$mobileconfig = replaceUserAndCert($template_file, $username, $pkcs12UUID);
	
	// Add the supporting certificate payload dictionaries (if any) to the profile
	if ($certDictArray) {
		foreach ($certDictArray as $certDict) {
			$mobileconfig->getValue()->get(PAYLOAD_CONTENT)->add($certDict);
		}
	}
	
	// Add the PKCS12 payload dictionary to the profile
	$mobileconfig->getValue()->get(PAYLOAD_CONTENT)->add($pkcs12Dict);
	
	// Prepend the user's name to the filename
	$profile = "profiles/" . $username . "." . $template_id . ".mobileconfig";
	
	// Save the unsigned profile to the file system
	$mobileconfig->saveXML( $profile );
	
	// Display the profile
	if ($debug) {
		$xml = file_get_contents( $profile );
		echo 'Profile: <br /><pre>' . xmlpp($xml, TRUE) . '</pre>';
	}
	
	// Sign the profile
	signProfile($profile);
	
	// Redirect the user to the signed profile
	if (!$debug) {
		header( 'Location: ' . $profile );
	}
}

?>

<?php
require_once 'constants.php';
require_once "CFPropertyList/classes/CFPropertyList/CFPropertyList.php";
require_once "RestRequest.inc.php";

function gen_uuid()
{
	return sprintf( '%04x%04x-%04x-%04x-%04x-%04x%04x%04x',
		// 32 bits for "time_low"
		mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff ),

		// 16 bits for "time_mid"
		mt_rand( 0, 0xffff ),

		// 16 bits for "time_hi_and_version",
		// four most significant bits holds version number 4
		mt_rand( 0, 0x0fff ) | 0x4000,

		// 16 bits, 8 bits for "clk_seq_hi_res",
		// 8 bits for "clk_seq_low",
		// two most significant bits holds zero and one for variant DCE1.1
		mt_rand( 0, 0x3fff ) | 0x8000,

		// 48 bits for "node"
		mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff )
	);
}

/** Prettifies an XML string into a human-readable and indented work of art
 *  @param string $xml The XML as a string
 *  @param boolean $html_output True if the output should be escaped (for use in HTML)
 */
function xmlpp($xml, $html_output=false) {
	$xml_obj = new SimpleXMLElement($xml);
	$level = 4;
	$indent = 0; // current indentation level
	$pretty = array();

	// get an array containing each XML element
	$xml = explode("\n", preg_replace('/>\s*</', ">\n<", $xml_obj->asXML()));

	// shift off opening XML tag if present
	if (count($xml) && preg_match('/^<\?\s*xml/', $xml[0])) {
		$pretty[] = array_shift($xml);
	}

	foreach ($xml as $el) {
		if (preg_match('/^<([\w])+[^>\/]*>$/U', $el)) {
			// opening tag, increase indent
			$pretty[] = str_repeat(' ', $indent) . $el;
			$indent += $level;
		} else {
			if (preg_match('/^<\/.+>$/', $el)) {
				$indent -= $level;  // closing tag, decrease indent
			}
			if ($indent < 0) {
				$indent += $level;
			}
			$pretty[] = str_repeat(' ', $indent) . $el;
		}
	}
	$xml = implode("\n", $pretty);
	return ($html_output) ? htmlentities($xml) : $xml;
}

function getProfileID($filename)
{
	$plist = new CFPropertyList\CFPropertyList($filename);
	$profileID = $plist->getValue()->get(PAYLOAD_ID)->getValue();
	return $profileID;
}

function getCertificate($user, $pass, $cred2, $cred3, $cred4)
{
	// Build the request
	$url = CERT_SERVER_URL . "?Function=GetCertificate&LoginId={$user}&EncryptPkcs12=Yes&LoginPwd={$pass}&LoginCred2={$cred2}&LoginCred3={$cred3}&LoginCred4={$cred4}&CaName=incommontest.org";
	$request = new RestRequest($url, 'GET');

	// Execute the request
	$request->execute();

	// Check for errors
	$responseInfo = $request->getResponseInfo();
	if ( 400 == $responseInfo['http_code'] ) {
		header( 'Location: ' .  LOGIN_ERROR_PAGE);
		return NULL;
	}

	// Extract the cert from the response
	$responseBody = $request->getResponseBody();
	$InCommonProvision = simplexml_load_string($responseBody);

	// Save what we got for inspection
	$data = base64_decode($InCommonProvision->UserCertificate[0]->pkcs12);
	$fh = fopen("certificates/$user.p12", 'w');
	fwrite($fh, $data);
	fclose($fh);

	// Return whatever we got back
	return $data;
}

function buildProfile($profileIdentifier, $profileOrg, $payloadArray)
{
	$myUUID = gen_uuid();

	$plist = new CFPropertyList\CFPropertyList();
	$plist->add( $dict = new CFPropertyList\CFDictionary() );
	$dict->add( PAYLOAD_CONTENT, $payloadArray );
	$dict->add( PAYLOAD_DESC, new CFPropertyList\CFString( 'This profile provides you with your secure identity and allows access to the Wi-Fi network.' ) );
	$dict->add( PAYLOAD_NAME, new CFPropertyList\CFString( 'InCommon Cert Profile' ) );
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier ) );
	$dict->add( PAYLOAD_ORG, new CFPropertyList\CFString( $profileOrg ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_CONFIG ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	return $plist;
}

function signProfile($filename)
{
	$outFilename = $filename . ".tmp";

	// try signing the plain XML profile
	if (openssl_pkcs7_sign($filename, $outFilename, SIGN_CERT, array(PRIV_KEY, ""), array(), 0, EXTRA_CERTS)) {
		// get the data back from the filesystem
		$signedString = file_get_contents($outFilename);

		// trim the fat
		$trimmedString = preg_replace('/(.+\n)+\n/', '', $signedString, 1);

		// convert to binary (DER)
		$decodedString = base64_decode($trimmedString);

		// write the file back to the filesystem (using the filename originally given)
		$fh = fopen($filename, 'w');
		fwrite($fh, $decodedString);
		fclose($fh);

		// delete the temporary file
		unlink($outFilename);

		return TRUE;
	} else {
		return FALSE;
	}
}

function replaceUserAndCert($filename, $username, $pkcs12UUID)
{
	// An array of the patterns to match
	$pattern = array(USER_PLACEHOLDER, PKCS12_PLACEHOLDER);

	// An array of the respective replacements
	$replacement = array($username, $pkcs12UUID);

	// Read in the file
	$subject = file_get_contents($filename);

	// Perform the replace
	$plistString = preg_replace($pattern, $replacement, $subject);

	// create a new Property list based on this new string
	$plist = new CFPropertyList\CFPropertyList();
	$plist->parse($plistString);

	return $plist;
}

// !Payload Builders

function passwordPolicyPayload($profileIdentifier, $policyDict)
{
	// this payload's UUID
	$myUUID = gen_uuid();

	$dict = new CFPropertyList\CFDictionary();

	// the required keys common to all payloads
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier . '.passcodepolicy.' . $myUUID ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_PASSWORD ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	// the keys specific to this payload
	foreach ($policyDict as $key => $value) {
		$dict->add( $key->getValue(), $value);
	}

	return $dict;
}

function pkcs1Payload($profileIdentifier, $certData)
{
	// this payload's UUID
	$myUUID = gen_uuid();

	$dict = new CFPropertyList\CFDictionary();

	// The required keys common to all payloads
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier . '.certificate.' . $myUUID ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_PKCS1 ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	// the keys specific to this payload
	$dict->add( PAYLOAD_CONTENT, new CFPropertyList\CFData($certData, TRUE) );

	return $dict;
}

function pkcs12Payload($profileIdentifier, $certData)
{
	// this payload's UUID
	$myUUID = gen_uuid();

	$dict = new CFPropertyList\CFDictionary();

	// the required keys common to all payloads
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier . '.certificate.' . $myUUID ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_PKCS12 ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	// not required, but helpful
	$dict->add( PAYLOAD_NAME, new CFPropertyList\CFString( "InCert Identity" ) );
	$dict->add( PAYLOAD_DESC, new CFPropertyList\CFString( "My Certificate & Private Key" ) );

	// the keys specific to this payload
	$dict->add( PAYLOAD_CONTENT, new CFPropertyList\CFData($certData) );

	return $dict;
}

function rootPayload($profileIdentifier, $certData)
{
	// this payload's UUID
	$myUUID = gen_uuid();

	$dict = new CFPropertyList\CFDictionary();

	// The required keys common to all payloads
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier . '.certificate.' . $myUUID ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_ROOT ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	// the keys specific to this payload
	$dict->add( PAYLOAD_CONTENT, new CFPropertyList\CFData($certData, TRUE) );

	return $dict;
}

function wifiPayload($profileIdentifier, $contentDict)
{
	// this payload's UUID
	$myUUID = gen_uuid();

	$dict = new CFPropertyList\CFDictionary();

	// the required keys common to all payloads
	$dict->add( PAYLOAD_ID, new CFPropertyList\CFString( $profileIdentifier . '.interfaces.' . $myUUID ) );
	$dict->add( PAYLOAD_TYPE, new CFPropertyList\CFString( TYPE_WIFI ) );
	$dict->add( PAYLOAD_UUID, new CFPropertyList\CFString( $myUUID ) );
	$dict->add( PAYLOAD_VER, new CFPropertyList\CFNumber( 1 ) );

	// the keys specific to this payload
	$dict->add( WIFI_SSID_STR, $contentDict->get(WIFI_SSID_STR) );
	$dict->add( WIFI_ENCRYPT_TYPE, $contentDict->get(WIFI_ENCRYPT_TYPE) );
	if ( $EAPDict = $contentDict->get(WIFI_EAP_CONFIG) ) {};


	$dict->add( PAYLOAD_CERT_UUID, new CFPropertyList\CFString( $certificateUUID ) );
	$dict->add( EAP_CONFIG, $EAPdict = new CFPropertyList\CFDictionary() );

	$EAPdict->add( ACCEPT_EAP, new CFPropertyList\CFArray( new CFNumber( 13 ) ) );
	$EAPdict->add( PAYLOAD_CERT_ANCHOR_UUID, new CFPropertyList\CFArray( $anchorUUID ) );
	$EAPdict->add( TLS_TRUSTED_NAMES, new CFPropertyList\CFArray( new CFPropertyList\CFString( '*.noc.iu.edu' ) ) );

	return $dict;
}

?>

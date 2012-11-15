<?php

// !Placeholder Constants

define("USER_PLACEHOLDER", '/\$username\$/');
define("PKCS12_PLACEHOLDER", '/\$pkcs12UUID\$/');

// !File Locations/Names

define("SIGN_CERT", "file://ssl/signcert.pem");
define("PRIV_KEY", "file://ssl/privkey.pem");
define("EXTRA_CERTS", "ssl/extracerts.pem");
define("LOGIN_ERROR_PAGE", "login_err.html");
define("TEMPLATE_ERROR_PAGE", "template_err.html");

// !Certificate Server URL
define("CERT_SERVER_URL", "https://bd3.itc.virginia.edu/cgi-bin/incommon_server.py");

// The following is taken from
// Configuration Profile Key Reference
// http://developer.apple.com/library/ios/#featuredarticles/iPhoneConfigurationProfileRef/Introduction/Introduction.html
//
// See also
// Over-the-Air Profile Delivery and Configuration
// http://developer.apple.com/library/ios/#documentation/NetworkingInternet/Conceptual/iPhoneOTAConfiguration/Introduction/Introduction.html

// !Profile & Payload Keys
define("PAYLOAD_ID", "PayloadIdentifier");
define("PAYLOAD_TYPE", "PayloadType");
define("PAYLOAD_UUID", "PayloadUUID");
define("PAYLOAD_VER", "PayloadVersion");
define("PAYLOAD_DESC", "PayloadDescription"); // optional
define("PAYLOAD_NAME", "PayloadDisplayName"); // optional
define("PAYLOAD_ENABLED", "PayloadEnabled");  // optional
define("PAYLOAD_ORG", "PayloadOrganization"); // optional

// !Profile Keys
define("CONSENT_TEXT", "ConsentText");        // optional
define("DURATION_UNTIL_REMOVAL", "DurationUntilRemoval");   // optional
define("HAS_REMOVAL_PASSCODE", "HasRemovalPasscode");    // optional
define("IS_ENCRYPTED", "IsEncrypted");        // optional
define("PAYLOAD_CONTENT", "PayloadContent");      // optional
define("PAYLOAD_REMOVAL_DISALLOWED", "PayloadRemovalDisallowed"); // optional
define("PAYLOAD_SCOPE", "PayloadScope");       // optional  Available in OS X v10.8 and later.
define("REMOVAL_DATE", "RemovalDate");        // optional

// !Payload Types
define("TYPE_CONFIG", "Configuration");
define("TYPE_REMOVAL", "com.apple.profileRemovalPassword");
define("TYPE_PASSWORD", "com.apple.mobiledevice.passwordpolicy");
define("TYPE_ID", "com.apple.configurationprofile.identification");
define("TYPE_MAIL", "com.apple.mail.managed");
define("TYPE_WEB", "com.apple.webClip.managed");
define("TYPE_PKCS1", "com.apple.security.pkcs1");
define("TYPE_PKCS12", "com.apple.security.pkcs12");
define("TYPE_SCEP", "com.apple.security.scep");
define("TYPE_EAS", "com.apple.eas.account"); // Exchange Active Sync (iOS)
define("TYPE_EWS", "com.apple.ews.account"); // Exchange Web Servies (OS X, Contacts only)
define("TYPE_VPN", "com.apple.vpn.managed");
define("TYPE_WIFI", "com.apple.wifi.managed");

//
// ! *Payload-Specific Property Keys*
//

// !Profile Removal Password Keys
define("REMOVAL_PASSWORD", "RemovalPassword"); // optional

// !Passcode Policy Keys
define("PASSWORD_ALLOW_SIMPLE", "allowSimple");   // optional
define("PASSWORD_FORCE_PIN", "forcePIN");    // optional
define("PASSWORD_MAX_FAILED", "maxFailedAttempts");  // optional
define("PASSWORD_MAX_GRACE", "maxGracePeriod");   // optional
define("PASSWORD_MAX_INACTIVE", "maxInactivity");  // optional
define("PASSWORD_MAX_PIN_AGE", "maxPINAgeInDays");  // optional
define("PASSWORD_MIN_COMPLEX", "minComplexChars");  // optional
define("PASSWORD_MIN_LENGTH", "minLength");    // optional
define("PASSWORD_PIN_HISTROY", "pinHistory");   // optional  This key is unavailable in OS X.
define("PASSWORD_REQUIRE_ALPHA", "requireAlphanumeric"); // optional

// !Identification Keys (This payload is not supported in iOS)
define("ID_MAIL", "EmailAddress");
define("ID_FULLNAME", "FullName");
define("ID_PASSWORD", "Password");
define("ID_PROMPT", "Prompt");
define("ID_USERNAME", "UserName");

// !Email Keys
define("MAIL_DISABLE_RECENTS_SYNC", "disableMailRecentsSyncing");   // Available only in iOS 6.0 and later.
define("MAIL_TYPE", "EmailAccountType");
define("MAIL_ADDRESS", "EmailAddress");
define("MAIL_INCOMING_AUTH", "IncomingMailServerAuthentication");
define("MAIL_INCOMING_HOST", "IncomingMailServerHostName");
define("MAIL_INCOMING_USER", "IncomingMailServerUsername");
define("MAIL_OUTGOING_AUTH", "OutgoingMailServerAuthentication");
define("MAIL_OUTGOING_HOST", "OutgoingMailServerHostName");
define("MAIL_OUTGOING_USER", "OutgoingMailServerUsername");
define("MAIL_DESC", "EmailAccountDescription");        // optional
define("MAIL_NAME", "EmailAccountName");         // optional
define("MAIL_INCOMING_PORT", "IncomingMailServerPortNumber");    // optional
define("MAIL_INCOMING_SSL", "IncomingMailServerUseSSL");     // optional
define("MAIL_INCOMING_PASSWORD", "IncomingPassword");      // optional
define("MAIL_OUTGOING_PORT", "OutgoingMailServerPortNumber");    // optional
define("MAIL_OUTGOING_SSL", "OutgoingMailServerUseSSL");     // optional
define("MAIL_OUTGOING_PASSWORD", "OutgoingPassword");      // optional
define("MAIL_SAME_PASSWORD", "OutgoingPasswordSameAsIncomingPassword");  // optional
define("MAIL_PREVENT_APP_SHEET", "PreventAppSheet");      // optional  Available only in iOS 5.0 and later.
define("MAIL_PREVENT_MOVE", "PreventMove");         // optional  Available only in iOS 5.0 and later.
define("MAIL_SMIME_ENABLED", "SMIMEEnabled");        // optional  Available only in iOS 5.0 and later.
define("MAIL_SMIME_ENCRYPT_CERT_UUID", "SMIMEEncryptionCertificateUUID"); // optional  Available only in iOS 5.0 and later.
define("MAIL_SMIME_SIGN_CERT_UUID", "SMIMESigningCertificateUUID");   // optional  Available only in iOS 5.0 and later.

// !Web Clip Keys
define("WEB_LABEL", "Label");
define("WEB_URL", "URL");
define("WEB_ICON", "Icon");        // optional
define("WEB_IS_REMOVABLE", "IsRemovable"); // optional

// !SCEP Keys
define("SCEP_URL", "URL");
define("SCEP_NAME", "Name");   // optional
define("SCEP_SUBJECT", "Subject");  // optional
define("SCEP_CHALLENGE", "Challenge"); // optional
define("SCEP_KEY_SIZE", "Keysize");  // optional
define("SCEP_KEY_TYPE", "Key Type"); // optional
define("SCEP_KEY_USAGE", "Key Usage"); // optional  Available only in iOS 4 and later.

// !Exchange Keys
define("EXCHANGE_ADDRESS", "EmailAddress");          // In OS X, this key is required.
define("EXCHANGE_HOST", "Host");               // In OS X, this key is required.
define("EXCHANGE_USER", "UserName");              // In OS X, this key is required.
define("EXCHANGE_CERT_UUID", "PayloadCertificateUUID");       // Available in iOS 5.0 and later.
define("EXCHANGE_DISABLE_RECENTS_SYNC", "disableMailRecentsSyncing");   // Available only in iOS 6.0 and later.
define("EXCHANGE_SSL", "SSL");                // optional
define("EXCHANGE_PASSWORD", "Password");             // optional
define("EXCHANGE_CERT", "Certificate");           // optional
define("EXCHANGE_CERT_NAME", "CertificateName");        // optional
define("EXCHANGE_CERT_PASSWORD", "CertificatePassword");      // optional
define("EXCHANGE_PREVENT_APP_SHEET", "PreventAppSheet");      // optional  Available in iOS 5.0 and later.
define("EXCHANGE_PREVENT_MOVE", "PreventMove");         // optional  Available in iOS 5.0 and later.
define("EXCHANGE_SMIME_ENABLED", "SMIMEEnabled");        // optional  Available in iOS 5.0 and later.
define("EXCHANGE_SMIME_ENCRYPT_CERT_UUID", "SMIMEEncryptionCertificateUUID"); // optional  Available in iOS 5.0 and later.
define("EXCHANGE_SMIME_SIGN_CERT_UUID", "SMIMESigningCertificateUUID");   // optional  Available in iOS 5.0 and later.

// !VPN Keys
define("VPN_NAME", "UserDefinedName");
define("VPN_OVERRIDE", "OverridePrimary");
define("VPN_TYPE", "VPNType");
define("VPN_SUBTYPE", "VPNSubType");
define("VPN_IPSEC", "IPSec");
define("VPN_IPSEC_ADDRESS", "RemoteAddress");    // Used for Cisco IPSec.
define("VPN_IPSEC_AUTH_METHOD", "AuthenticationMethod"); // Used for L2TP and Cisco IPSec.
define("VPN_IPSEC_XAUTH_USER", "XAuthName");    // Used for Cisco IPSec.
define("VPN_IPSEC_XAUTH_ENABLED", "XAuthEnabled");   // Used for Cisco IPSec.
define("VPN_IPSEC_LOCAL_ID", "LocalIdentifier");   // Used for Cisco IPSec.
define("VPN_IPSEC_LOCAL_ID_TYPE", "LocalIdentifierType"); // Used for L2TP and Cisco IPSec.
define("VPN_IPSEC_SHARED_SECRET", "SharedSecret");   // Used for L2TP and Cisco IPSec.
define("VPN_IPSEC_CERT_UUID", "PayloadCertificateUUID"); // Used for Cisco IPSec.
define("VPN_IPSEC_PIN_PROMPT", "PromptForVPNPIN");   // Used for Cisco IPSec.
define("VPN_PPP", "PPP");
define("VPN_PPP_USER", "AuthName");     // Used for L2TP and PPTP.
define("VPN_PPP_PASSWORD", "AuthPassword");   // optional  Used for L2TP and PPTP.
define("VPN_PPP_TOKEN", "TokenCard");    // Used for L2TP.
define("VPN_PPP_ADDRESS", "CommRemoteAddress");  // Used for L2TP and PPTP.
define("VPN_PPP_AUTH_PLUGINS", "AuthEAPPlugins"); // Used for L2TP and PPTP.
define("VPN_PPP_AUTH_PROTOCOL", "AuthProtocol"); // Used for L2TP and PPTP.
define("VPN_PPP_CCPMPPE40", "CCPMPPE40Enabled"); // Used for PPTP.
define("VPN_PPP_CCPMPPE128", "CCPMPPE128Enabled"); // Used for PPTP.
define("VPN_PPP_CCP_ENABLED", "CCPEnabled");  // Used for PPTP.
define("VPN_VPN", "VPN");
define("VPN_VPN_AUTH_METHOD", "AuthenticationMethod");
define("VPN_VPN_ADDRESS", "RemoteAddress");

// !Wi-Fi Keys
define("WIFI_ENCRYPT_TYPE", "EncryptionType");   // Available in iOS 4.0 and later; the None value is available in iOS 5.0 and later.
define("WIFI_HIDDEN_NETWORK", "HIDDEN_NETWORK");
define("WIFI_SSID_STR", "SSID_STR");
define("WIFI_AUTO_JOIN", "AutoJoin");     // optional  Available in iOS 5.0 and later.
define("WIFI_PASSWORD", "Password");     // optional
define("WIFI_CERT_UUID", "PayloadCertificateUUID");  // optional
define("WIFI_PROXY_TYPE", "ProxyType");     // optional  Available in iOS 5.0 and later.
define("WIFI_PROXY_SERVER", "ProxyServer");
define("WIFI_PROXY_PORT", "ProxyServerPort");
define("WIFI_PROXY_USERNAME", "ProxyUsername"); // optional
define("WIFI_PROXY_PASSWORD", "ProxyPassword"); // optional
define("WIFI_PROXY_PACURL", "ProxyPACURL");  // optional
define("WIFI_EAP_CONFIG", "EAPClientConfiguration"); // optional
define("WIFI_EAP_ACCEPT_TYPES", "AcceptEAPTypes");
define("WIFI_EAP_CERT_ANCHOR_UUID", "PayloadCertificateAnchorUUID"); // optional
define("WIFI_EAP_TLS_TRUSTED_NAMES", "TLSTrustedServerNames");   // optional
define("WIFI_EAP_TLS_TRUST_EXCEPTIONS", "TLSAllowTrustExceptions");  // optional
define("WIFI_EAP_TTLS_INNER_AUTH", "TTLSInnerAuthentication");   // optional
define("WIFI_EAP_OUTER_ID", "OuterIdentity");       // optional
define("WIFI_EAP_USERNAME", "UserName");        // optional
define("WIFI_EAP_FAST_PAC", "EAPFASTUsePAC");        // optional
define("WIFI_EAP_FAST_PROV_PAC", "EAPFASTProvisionPAC");     // optional
define("WIFI_EAP_FAST_PROV_PAC_ANON", "EAPFASTProvisionPACAnonymously"); // optional

?>
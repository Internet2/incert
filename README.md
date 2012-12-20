InCommon Personal Certificate Provisioning Tool
===============================================


Overview
--------
This project is the OS X/iOS componenet of the [InCommon Personal Certificate Provisioning and Application Setup Tool](https://spaces.internet2.edu/x/f66KAQ).

It's purpose is to generate a signed configuration profile for use by iOS devices and OS X clients (Lion and later) which includes the user's personal identity certificate and configures other services such as email, wireless, and passcode requirements.


API
---
API examples are found [here](https://bd3.itc.virginia.edu/incommon/).


Templates
---------
The tool starts which a base configuration profile template, adds the user's certificate as another payload, and replaces any tokens in the template with real values. The current tokens are:

	$username$
	$pkcs12UUID$
 
Any occurence of these token in the template will be replaced with the user's actual username and the payload UUID of the user's actual certificate.
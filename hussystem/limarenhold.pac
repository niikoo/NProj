function FindProxyForURL(url, host) {
	// LIMA RENHOLD PROXY AUTOCONFIG - AV OMMUNDSEN SERVICES
	// Prøv proxy for cache, hvis ikke den er tilgjengelig, gå direkte til nettet.
	if (dnsDomainIs(host, "google.com") || dnsDomainIs(host, "www.google.com"))
        return "DIRECT";
	if (isInNet(myIpAddress(), "192.168.1.0", "255.255.255.0"))
		return "PROXY 192.168.1.2:3128; DIRECT";
	else
		return "DIRECT";
}
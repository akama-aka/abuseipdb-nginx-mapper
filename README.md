# AbuseIPDB Nginx Mapping File

## 1. Introduction

This repository creates a nginx.conf file after execution that contains a Nginx `mapping` which can then be used as a
blocklist
for your Nginx WAF.

## 2. Technical Information

## 2.1 Third Party Usage

This program uses the blocklist from https://github.com/borestad/blocklist-abuseipdb which in turn uses a list of
IP addresses from the last month that comes from AbuseIPDB.

## 2.1.1 Why just the last month?

Even though there are several lists with different intervals, I currently decided to use only the last month.

The reason for this decision is that 30 days is, in my opinion, a perfect time interval since they are already only the
100% matches and therefore few changes there either. Additionally, I like to conserve API resources.

## 3. Build Process

## 3.1 Self Compiling

1. To build the program yourself, you need to have dotnet 8 installed on your device. A guide for .NET 6 can be
   found [here](https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)
   _https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website_
2. After installation, you can clone this repository with the command
   `git clone https://github.com/akama-aka/abuseipdb-nginx-mapper.git`
3. If this was successful, navigate to the newly created folder named `abuseipdb-nginx-mapper` and run the command
   `dotnet build`
4. After the build process you can run `dotnet run` and a file named `nginx.conf` should appear in the
   `bin/Debug/net8.0` folder
5. Add this file to your Nginx configuration.

## 4. Usage

To use the map, you need to insert the configuration into your nginx host configuration using the `include` command.
This can look like this:

```config
server {
    listen 443;
    ...
    include PATH_TO_THE_FILE.conf;
    location / {
        if($mitigate_ip_bad)
            deny;
        }
    }
    ...
}
```

#!/bin/bash

source ./project-vars.sh

sourceDir=$1
remoteHost=$FTP_REMOTE_HOST
username=$FTP_USERNAME
password=$FTP_PASSWORD

echo "Uploading to ftp://$FTP_REMOTE_HOST/$ProjectFtpDest/$FTP_DEST"
echo "Logging in as $FTP_USERNAME"

ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $ProjectFtpDest/$FTP_DEST $1/*


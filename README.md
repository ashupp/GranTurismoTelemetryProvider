# GranTurismoTelemetryProvider
Gran Turismo 7 Telemetry Provider for SimFeedback / SFX-100

Please support this great project.  
https://opensfx.com

## Please read the installation instructions below BEFORE download 

## Warning  
**Unpredicted movement of you SFX-100 could happen at any time when this plugin is enabled.**  
I am not responsible of any damages caused by usage of this plugin!  
**You have been warned!**  

## Requirements
- Playstation 4 or 5 (tested only on 5) with Gran Turismo 7

## Installation
- Do not click on Download on this page. Download the file from the release tab or use the following link:
  - [latest GranTurismoTelemetryProvider.zip](https://github.com/ashupp/GranTurismoTelemetryProvider/releases/latest/download/GranTurismoTelemetryProvider.zip)  
  
- Open zip archive
- Put contents of folder "provider" into folder "SimFeedbackFolder/provider"  
- Put contents of folder "img" to folder "SimFeedbackFolder/img"  
- Put contents of folder "profiles" to folder "SimFeedbackFolder/profiles"  
- Extract included PDTools-SimulatorInterfaceTest.zip somewhere

## How to us
- Start custom PDTools-SimulatorInterfaceTest on command line before starting SimFeedback `SimulatorInterfaceTest.exe 192.168.1.47 --udpsend` where 192.168.1.47 should be the ip of your playstation.
- Start up Simfeedback and launch the Gran Turismo telemetry provider

## Third Party Contents
- Contains a custom version of https://github.com/Nenkai/PDTools - Without his tools this telemetry provider would not be possible. Custom version changes here: https://github.com/Nenkai/PDTools/compare/master...ashupp:PDTools:master

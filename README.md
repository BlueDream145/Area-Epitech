# Area

The goal of this project is to discover, as a whole, the software platform that you have chosen (Java, .NET,node.js) through the creation of a business application.To do this, you must implement a software suite that functions similar to that of IFTTT and/or Zapier.This software suite will be broken into three parts :
•An application serverto implement all the features listed below (seeFeatures)
•A web client to use the application from your browser by querying the application server
•A mobile client to use the application from your phone by querying the application server
 
	# Server
		Folder = Area/Area.Server
		
	# Mobile Client
		Folder = Area/Area.MobileClient
			~IOS lib: Area/Area.MobileClient.IOS
			~Android lib: Area/Area.MobileClient.Android

	# Web Client
		Folder = Web
	
# Usage

	Comment lancer l'application ?
	
		- sudo docker-compose up --build
		
# FAQ

	Le port 3306 est déjà utilisé ? Un précédent container est toujours en cours d'exécution
	
		- sudo docker ps
		- sudo docker stop {container}
		- sudo docker rm {container}

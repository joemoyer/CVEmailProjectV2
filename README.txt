Setup:

	-Pull project from the repository

	-Find or create a test gmail account to send emails from
		>Sign into this google account and find the "Manage your Google Account" screen
		>Navigate to "App Passwords" using the search bar and create a new test app
		>Copy the 16 character string, this will be used as our authentication in appsettings.json

	-Create an SQL Server and run the script located in the repository named "CreateTableScript.sql"

	-Open the appsettings.json file and replace DBConnectionStringHere, EmailAddressHere, AccountKeyHere
		>DBConnectionStringHere = This is where you place the connection string for the previously created database
			*To get mine to work, I included the Trusted_Connection=True flag
		>EmailAddressHere = Address of host email selected earlier that will be sending the emails
		>AccountKeyHere = This is where the 16 character string from earler will go

	-Run the project using `dotnet run`

	-To send an email, open Postman and make the call `<HostURL>/SendEmail`
		>This API call is a POST request that expects 3 headers: EmailAddress, Subject, and Body
			*EmailAddress = Address of recipient email
			*Subject = Subject line text for email
			*Body = Body of email

Specifications Addressed:

	-The EmailSender class should be reusable throughout different applications and entry points with
		minimal setup. It currently calls the DBController but this can be removed or easily implemented
		in another project. 
	-Email recipient, subject, body, and date are logged in the SQL database.
	-Failed email attempts are handled by a recursive function that retries the method up to 3 times.
	-The project is designed to be called from Postman as an API, but could still be called form a console application
	-The front end was scrapped as I ran out of time.


	
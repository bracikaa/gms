# gms
An app that manages a fitness centre

**Requirements**


*Logging in (application administrator both levels):* In order to log in, the
application administrator has to know his/hers credentials (the username and the
password the account has been registered). The application will prompt the user to
provide credentials right at the startup, inside the Log In Form. If the process has
passed successfully a welcoming message box will pop out, informing the
administrator that he/she is now fully able to use the application based on the
authorization level. Otherwise, a message box with an accompanying error
message will pop up, informing the user on the reasons why the application is
unable to proceed.

Tracking the authorization levels (application administrator both levels):
Depending on the authorization level (referenced as ‘1’ or ‘2’ in the database) the
application administrator can have different controls in the application. The
application administrator titled as ‘administrator’ or ‘1’ can manage the members
and their memberships in the application, while the application administrator titled
as ‘trainer’ or ‘2’ additionally can check on the training programs, gym shop and
measurement history.

*Adding member (application administrator both levels):* In order to add a member,
the user has to have a tag card given by the fitness center employee. The fitness
center employee then inputs the information about the member and scans the card
with the RFID reader. In order to successfully finish the process of adding a new
member the card has to be new and the data on the card not to be found in the
database. The administrator also keeps a track of the validity of data – but even if
he misses some fields, the Add Member Form has built-in validation mechanism
73that will explain to the administrator why a certain task might have failed. If the
process has passed successfully adequate message should appear on the screen.
Validating the member (application administrator both levels): In order to validate
already existing member in the fitness center, the member has to scan the tag with
the RFID reader, if the card has been found in the database system the application
will act accordingly by opening the form and prompting the application
administrator to validate the member. The member can accordingly extend the
duration of his fitness center membership by clicking on “Extend the membership”
button. If passed successfully a message will pop up informing the application
administrator of the action.

*Checking the training schedule (application administrator level 2):* In order to
check on the training schedule the application administrator has to have the
additional database field of ‘trainer’. If the application administrator clicks on the
tab that says “Training” he/she should see all the training sessions, which should
display more information if the administrator clicks on any of them. If the
administrator brings up the RFID card of an already registered member he can
enroll the member in the training slot. The database then inserts a new row in the
MembershipEnrollment table where information about the training name, type,
price and member name and can be found. If the process passes successfully the
application administrator should get a positive return message/statement.

*Searching for a specific member (application administrator both levels):* In order
to search for a specific member the application administrator firstly clicks on the
tab “Search Members” and then inputs the search query in the search form and if
necessary filter additional data such as the surname or the gender of the member.
The Search Member form shows additional information about the members at all
times, both displaying general info about all the members but also about individual
or clicked member. The necessity for this functionality is that there are existing
74members in the database. If the process passes successfully the application
administrator has the information about the member on the screen.

*Report generation (application administrator both levels):* In order to generate a
report for a certain member the application administrator has to click on the “Sheet
Report” tab and then after filtering the report by selecting the time frame or the
individual member the application administrator clicks on the “Generate Report”
button. If the process has passes successfully the report has been generated and
saved in a document file for further use or modification.

*Entering measurements (application administrator level 2):* In order to enter
measurements, the application administrator (or alternatively the trainer) needs to
click on the “Body Stats” tab and select a gym member. The gym members
measurements will appear on the screen if they were previously entered. The
application administrator then can update the values accordingly to the
measurements of the member. If the process passes successfully the database will
be updated with a new row containing the updated measurements.

*Buying an item (application administrator level 2):* In order to sell an item the
application administrator needs to have a member’s RFID card and he needs to
click on “Shop” tab and select an item. If the process passes successfully, a new
row should be inserted into ShopPayments table in the database with information
about the user name, user id, and the item name, item id and the current stock
amount of the item. Additionally, a receipt is generated.

*Changing administrator information (application administrator both levels):* In
addition to having administrator privileges the application administrator can
change information about himself or about the application itself such as changing
the username and password of the administrator. If the process passes successfully
the application administrator sees the change with the according message.


**Screenshots**

**Custom Message Box**

![Custom Message Box](https://i.imgur.com/sfvcnYu.png)

**App screenshot**

![App Screenshot](https://i.imgur.com/cWuNYcy.png)

**List of Members**
![List of Members](https://i.imgur.com/67NWpuL.png)

**Measurement history form**
![Measurement History form](https://i.imgur.com/37xAQ7E.png)




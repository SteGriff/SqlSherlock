# Sherlock cookbook

## General ideas

To create flows, put subdirectories within the `sql` dir. If you do so, each directory name becomes a flow name, in which you may put spaces. The flow name should give an idea of what it's going to be used for. E.g. 'Find brokers' or 'Check product relationships'. 

Each step within the flow is a SQL file, and the file name becomes the step name. It starts with a number and a dot, to set ordering. The step name should be like a prompt indicating what it will do and what data is needed. E.g. '0. Get brokers by email.sql' or '2. Dry run - create new CRM key.sql'. As you can see, you can uses spaces and other handy characters to make the step name readable.

Parameter names are more constrained. As these are SQL variables, their names can contain underscores but not spaces, e.g. `declare @EnterAnID_OrZero int`. These will always look a little ugly in the web view, but (imo) it retains an important amount of connectedness for the user; it shows they are running a technical process which is "close to the metal".


## Just select

Write a normal select statement. It can include joins. Add one or more line comments to label the output.

`sql/Check CRMs/0. Get all CRMs.sql`

```sql
-- All the CRMs:
select top 1000 
ID,
CrmName,
CrmKey,
IsDeleted, 
IntegratorUrl
from CrmApiKey
```

## Select with param(s)

`sql/Find brokers/0.Get Brokers by Email Domain.sql`

```sql
-- Here are the first 100 brokers matching that email fragment

declare @EmailDomain nvarchar(255)

select top 100 
Code, 
FullName, 
BranchNo, 
BrokerNo,
EmailAddress,
IsDeleted, 
CONCAT('https://sales.example.org/sell?code=', Code) as [Link] 
from Brokers where EmailAddress like '%' + @EmailDomain
```


## Select with optional params

## Insert data

## Optionally delete then insert

Imagine a flow that manages a ban list or similar. You could have a flow like this:

 + 0. List all bans.sql
 + 1. Delete an item.sql
 + 2. Add an item.sql
 
You can make the delete step optional with a SQL `if`. If the user enters 0, nothing changes, but if they enter an ID, that record is removed. The param name `EnterAnID_OrZero` gives the UI hint, and this can also be conveyed in instructions from the previous step.

`sql/Manage Bans/1. Delete an item.sql`

```sql
-- See results panel

declare @EnterAnID_OrZero int

set nocount off;

if @EnterAnID_OrZero > ''
begin
  delete from BannedVehicle where ID = @EnterAnID_OrZero
  select concat('Deleted ', @@ROWCOUNT , ' item(s)') as Result
end
else
  select 'Nothing to do' as Result
```

## Insert with an optional field

```sql
declare @WebhookUrl nvarchar(2048) = IIF(@OptionalWebhookUrl <> '', @OptionalWebhookUrl, NULL)
```

## Dry run insert

Select a 'pretend' record to show the user what their script run will look like. Use the comments to reflect this and get approval. Then make the next step do the actual insert.

```sql
-- The new manager will look like this. 
-- Proceed on the next step if you're sure.

declare @ManagerName nvarchar(100)
declare @ManagerKey nvarchar(20)
declare @OptionalWebhookUrl nvarchar(2048)
declare @WebhookUrl nvarchar(2048) = IIF(@OptionalWebhookUrl <> '', @OptionalWebhookUrl, NULL)

select 
@ManagerName as ManagerName,
@ManagerKey as ManagerKey,
0 as IsDeleted,
GETDATE() as Created,
@WebhookUrl as IntegratorUrl
```

## Confirmation box

Another tactic for a risky operation is a confirmation checkbox. And remembering to put limits on your `update` statements with `update top (1)`!

`sql/Users/3.Really activate the user.sql`

```sql
-- The User has been updated as follows:

declare @UserId int
declare @NewEmailAddress nvarchar(255)
declare @YesIWantToActivateTheUser bit

if @YesIWantToActivateTheUser = 1
begin
  update top (1) [Users]
  set EmailAddress = @NewEmailAddress,
  IsApproved = 1,
  LastUpdate = GETDATE()
  where UserID = @UserId

  select top 1 UserId, FirstName, Surname, EmailAddress, IsApproved, IsLockedOut, LastUpdate 
  from [Users] 
  where UserID = @UserId
end
else
  select 'You didn''t tick the box, so nothing happened' as Result
```


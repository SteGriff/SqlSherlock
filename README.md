# Sherlock

Sherlock makes it easy for the whole team to ask questions of the database, by providing an instant front-end onto SQL files prepared by devs, DBAs, or analysts.

It can be useful for troubleshooting config tables in a database or simply asking questions about the setup of entities, like, which products can this user see?

## Config

 0. Install the site in IIS with a .Net 4 application pool
 1. Set up a database connection in `web.config`
 2. Put some SQL files in the SQL directory or create folders for flows (see below)
 
## Query Flows

A Query Flow is a load of questions about the same topic. 

You can create multiple flows by putting SQL files in subdirectories, and users will see a dropdown in Sherlock to pick which flow they want to go down.

If you put SQL files directly in the `/sql` directory, they will be treated as a flow called 'Default', and you won't see the flow selection dropdown.

You might have a default flow with these queries:

```
/sql/0. Check the user exists and is active.sql
/sql/1. Check the user can see products.sql
/sql/2. Check the user has permission to add products.sql
```

Each query is a "step" in the flow with a Next and Previous button.

If you create subdirectories in the `/sql` directory, the directory names will be used as the **Flow names** and you won't see the Default flow any more. E.g.

```
/sql/Users/0. Check the user exists and is active.sql
/sql/Users/1. Check the user can see products.sql
/sql/Products/0. Check the product exists.sql
/sql/Products/1. Check the product is included in a listing.sql
```

## SQL

An SQL filename *must* start with a number followed by dot, and have a meaningful name in plain language, E.g. `4. One or more discount criteria exist.sql`

Sherlock does a little bit of parsing on your SQL files to:

 * Extract parameter definitions
 * Extract comments (optionally)
 * Remove `GO` statements

An SQL file *must* contain one or more `DECLARE` (or `declare`) statements with a name and type, but no default value:

 * `DECLARE @UserId int` = Right
 * `DECLARE @UserId int = 1001` = Wrong

Parameter names are not case sensitive.

You *may* add comments using `-- Inline` or `/* Block */` syntax - they will be pulled through into the heading of the results panel. Please note that extra asterisk `*` symbols will be stripped out of comments.

Each SQL file *must* run only one query - the parser will remove `GO` statements.

Your SQL files can have side effects if you want. Remember that they will be run everytime the user clicks the 'Next' button on the query, and can be re-run an unlimited number of times.

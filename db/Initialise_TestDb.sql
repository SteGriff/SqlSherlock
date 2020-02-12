create table Brand (
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	Code nvarchar(10),
	PublishedFrom datetime,
	PublishedTo datetime,
	Active bit
)

insert into Brand (Name, Code, PublishedFrom, PublishedTo, Active)
values
('ActiveBlast',	'AB', '2018-01-01', '2038-01-01', 1),
('Megalegs',	'ML', '2018-01-01', '2038-01-01', 0) -- Obsolete

create table ProductFamily (
	ID int identity(1,1) primary key,
	BrandID  int
		foreign key references Brand(ID)
		on delete cascade
		on update cascade,
	Name nvarchar(50),
	Code nvarchar(10),
	PublishedFrom datetime,
	PublishedTo datetime,
	Active bit
)

insert into ProductFamily (Name, BrandID, Code, PublishedFrom, PublishedTo, Active)
values
('ActiveBlast Commuter Gilet 2020',	1, 'ABCOMGIL20',	'2019-12-01', '2038-01-01', 1),
('ActiveBlast Storm Coat 2019',		1, 'ABSTORM19',		'2019-04-01', '2038-01-01', 1),
('ActiveBlast Dangerous Hat',		1, 'ABDGRHAT',		'2018-01-01', '2038-01-01', 0) -- Recalled

create table Product (
	ID int identity(1,1) primary key,
	ProductFamilyID int
		foreign key references ProductFamily(ID)
		on delete cascade
		on update cascade,
	Name nvarchar(50),
	Shape nvarchar(1),	-- M/F/U(ni)
	Size nvarchar(3),	-- 3XS/2XS/XS/S/M/L/XL/2XL/3XL
	PublishedFrom datetime,
	PublishedTo datetime,
	Active bit
)

insert into Product (ProductFamilyID, Name, Shape, Size, PublishedFrom, PublishedTo, Active)
values
(1, 'ActiveBlast Commuter Gilet Men''s Large', 'M', 'L', '2019-12-01', '2038-01-01', 1),
(1, 'ActiveBlast Commuter Gilet Women''s Large', 'F', 'L', '2019-12-01', '2038-01-01', 1),
(1, 'ActiveBlast Storm Coat XXS', 'U', '2XS', '2019-12-01', '2038-01-01', 0), -- Stopped production - lack of demand
(2, 'ActiveBlast Storm Coat Medium', 'U', 'M', '2019-12-01', '2038-01-01', 1),
(2, 'ActiveBlast Storm Coat Large', 'U', 'L', '2019-12-01', '2038-01-01', 1),
(2, 'ActiveBlast Storm Coat XL', 'U', 'XL', '2019-12-01', '2038-01-01', 1)

create table Seller (
	ID int identity(1,1) primary key,
	Name nvarchar(100),
	PublishedFrom datetime,
	PublishedTo datetime,
	Active bit
)

insert into Seller (Name, PublishedFrom, PublishedTo, Active)
values
('Outdoorsman Inc',	'2018-01-01', '2038-01-01', 1),
('GoCycling UK',	'2018-01-01', '2038-01-01', 1),
('CampingFraud Ltd','2018-01-01', '2038-01-01', 0) -- Ceased trading

create table SellingContract (
	ID int identity(1,1) primary key,
	SellerID int
		foreign key references Seller(ID)
		on delete cascade
		on update cascade,
	BrandID int null
		foreign key references Brand(ID)
		on delete cascade
		on update cascade,
	Comment nvarchar(255),
	PublishedFrom datetime,
	PublishedTo datetime,
	Active bit
)

insert into SellingContract (SellerID, BrandID, Comment, PublishedFrom, PublishedTo, Active)
values
(1, 1, 'Ongoing contract for Outdoorsman to sell ActiveBlast',	'2018-01-01', '2038-01-01', 1),
(2, 1, 'Ongoing contract for GoCycling to sell ActiveBlast',	'2018-01-01', '2038-01-01', 1),
(3, 1, 'Ongoing contract for CampingFraud to sell ActiveBlast',	'2018-01-01', '2038-01-01', 1) -- N.b. Contract is active but the Seller is not

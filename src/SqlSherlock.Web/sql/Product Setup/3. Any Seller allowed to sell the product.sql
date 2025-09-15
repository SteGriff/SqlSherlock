-- Are there any sellers configured to sell this ProductFamily?:
declare @ProductFamilyCode nvarchar(10)
select
s.ID as SellerID,
s.Name as SellerName,
sc.Comment,
b.Name as BrandName,
pf.Code as ProductFamilyCode
from Seller s
join SellingContract sc
on sc.SellerId = s.Id
join Brand b
on sc.BrandId = b.Id
join ProductFamily pf
on pf.BrandId = b.Id
where s.Active = 1
and sc.Active = 1
and pf.Active = 1
and b.Active = 1
and GETDATE() > pf.PublishedFrom
and GETDATE() < pf.PublishedTo
and pf.Code = @ProductFamilyCode

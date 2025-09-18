-- Are there any products enabled?:
declare @ProductFamilyCode nvarchar(10)
select
p.ID as ProductID,
pf.ID as ProductFamilyID,
p.Name as ProductName,
p.Shape,
p.Size,
pf.Code as ProductFamilyCode
from Product p
join ProductFamily pf
on p.ProductFamilyId = pf.Id
where p.Active = 1
and pf.Active = 1
and GETDATE() > p.PublishedFrom
and GETDATE() < p.PublishedTo
and pf.Code = @ProductFamilyCode

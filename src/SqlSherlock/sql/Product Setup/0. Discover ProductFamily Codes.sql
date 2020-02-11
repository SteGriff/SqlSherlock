-- Here are the product family codes matching your query.
-- To get everything, leave the input blank
declare @ProductFamilyCodeFragment nvarchar(10)
select
pf.ID as ProductFamilyID,
pf.Name,
pf.Code as ProductFamilyCode,
pf.Active
from ProductFamily pf
where pf.Code like '%' + @ProductFamilyCodeFragment + '%'
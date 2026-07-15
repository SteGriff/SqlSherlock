-- Here are the product family codes matching your query.
-- To get everything, leave the input blank

declare @ProductFamilyCodeFragment nvarchar(10)
declare @IncludeInactive bit

select
pf.ID as ProductFamilyID,
pf.Name,
pf.Code as ProductFamilyCode,
pf.Active
from ProductFamily pf
where pf.Code like '%' + IIF(@ProductFamilyCodeFragment is null, '', @ProductFamilyCodeFragment) + '%'
and (@IncludeInactive = 1 or pf.Active = 1)

select a.id,MAX(a.name)
from sysobjects a
left join syscomments b
on a.id = b.id
where a.xtype = 'p'
and b.[text] like '% exec%'
group by a.id
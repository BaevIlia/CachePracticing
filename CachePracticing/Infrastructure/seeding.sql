create extension if not exists pgcrypto;

INSERT INTO public."Products"
("Name", "Price", "Description", "CreatedAt", "UpdatedAt")
select
'Product ' || gs,
round((random()*1000)::numeric, 2),
'Description for a Product ' || gs || ': ' || encode(digest(random()::text, 'sha1'), 'hex'),
now() - (random() * interval '365 days'),
now() - (random() * interval '30 days')
from generate_series(1, 100000) gs;
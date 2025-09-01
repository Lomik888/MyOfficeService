create table if not exists departments
(
    id         serial primary key,
    name       varchar(150) not null,
    phone      varchar(12)  not null unique,
    company_id integer      not null,
    foreign key ("company_id") references public."companies" ("id") on delete cascade
);

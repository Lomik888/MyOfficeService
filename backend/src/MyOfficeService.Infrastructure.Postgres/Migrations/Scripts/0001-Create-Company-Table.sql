create table if not exists companies
(
    id    serial primary key,
    name  varchar(150) not null,
    phone varchar(12)  not null unique
);

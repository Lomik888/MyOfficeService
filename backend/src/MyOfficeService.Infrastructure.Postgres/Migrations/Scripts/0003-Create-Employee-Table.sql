create table if not exists employees
(
    id            serial primary key,
    name          varchar(50) not null,
    phone         varchar(12) not null unique,
    company_id    integer     not null,
    department_id integer     not null,
    passport      jsonb       not null,
    foreign key ("company_id") references public."companies" ("id") on delete cascade,
    foreign key ("department_id") references public."departments" ("id") on delete restrict
);

CREATE UNIQUE INDEX idx_passport_number_unique
    ON employees ((passport ->> 'number'));

CREATE OR REPLACE FUNCTION check_department_company()
    RETURNS TRIGGER AS
$$
BEGIN
    IF NOT EXISTS (SELECT 1
                   FROM departments
                   WHERE id = NEW.department_id
                     AND company_id = NEW.company_id) THEN
        RAISE EXCEPTION 'Department % does not belong to company %', NEW.department_id, NEW.company_id;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_check_department_company
    BEFORE INSERT OR UPDATE
    ON employees
    FOR EACH ROW
EXECUTE FUNCTION check_department_company();

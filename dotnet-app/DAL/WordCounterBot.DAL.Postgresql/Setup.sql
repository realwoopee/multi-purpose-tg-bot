create table users
(
    user_id    bigint not null
        constraint users_pk
            primary key,
    user_name  varchar,
    first_name varchar,
    last_name  varchar
);

create unique index users_user_id_uindex
    on users (user_id);

create table counters
(
    chat_id bigint not null,
    user_id bigint not null,
    counter bigint,
    constraint counters_pk
        primary key (chat_id, user_id)
);

create table counters_dated
(
    chat_id bigint not null,
    user_id bigint not null,
    date    date   not null,
    counter bigint,
    constraint counters_dated_pk
        primary key (chat_id, user_id, date)
);
CREATE TABLE IF NOT EXISTS contacts (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(256) NOT NULL,
    phone VARCHAR(32) NOT NULL,
    CONSTRAINT uq_contacts_email_phone UNIQUE (email, phone)
);

CREATE TABLE IF NOT EXISTS message_topics (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL
);

INSERT INTO message_topics (id, name) VALUES
    (1, 'Техподдержка'),
    (2, 'Продажи'),
    (3, 'Другое'),
    (4, 'Еще один пункт')
ON CONFLICT (id) DO NOTHING;

CREATE TABLE IF NOT EXISTS messages (
    id SERIAL PRIMARY KEY,
    contact_id INTEGER NOT NULL REFERENCES contacts(id) ON DELETE RESTRICT,
    topic_id INTEGER NOT NULL REFERENCES message_topics(id) ON DELETE RESTRICT,
    text TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL
);










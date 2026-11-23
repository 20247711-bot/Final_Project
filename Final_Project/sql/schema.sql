CREATE SCHEMA IF NOT EXISTS portfolio;

CREATE TABLE portfolio.course (
    course_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    course_code VARCHAR(20) NOT NULL,
    course_name VARCHAR(120) NOT NULL,
    description VARCHAR(500),
    course_year VARCHAR(20),

    CONSTRAINT uq_course_code UNIQUE (course_code)
);

CREATE TABLE portfolio.project (
    project_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    course_id INTEGER NOT NULL,
    title VARCHAR(150) NOT NULL,
    description VARCHAR(1000),
    youtube_url VARCHAR(500),

    CONSTRAINT fk_project_course
        FOREIGN KEY (course_id)
        REFERENCES portfolio.course(course_id)
        ON DELETE CASCADE
);

CREATE TABLE portfolio.project_image (
    image_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    project_id INTEGER NOT NULL,
    image_url VARCHAR(500) NOT NULL,

    CONSTRAINT fk_image_project
        FOREIGN KEY (project_id)
        REFERENCES portfolio.project(project_id)
        ON DELETE CASCADE,

    CONSTRAINT uq_project_image
        UNIQUE (project_id, image_url)
);
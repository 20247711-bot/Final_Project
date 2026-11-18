INSERT INTO portfolio.course(course_code, course_name, description) VALUES
('HUMCOM1','Human Computer Interaction','Foundational concepts in information technology.'),
('DATASG1','Data Structures and Algorithms','Data structures, algorithms, and problem solving.'),
('DBMWEB1','Database Management and Web Systems','Relational databases, SQL, and database design.')
ON CONFLICT (course_code) DO NOTHING;

INSERT INTO portfolio.project(course_id, project_title, description, video_url, completed_on)
SELECT course_id,'Sample Library System','A sample CRUD library management system built as a course project.',NULL,CURRENT_DATE
FROM portfolio.course WHERE course_code='DBMS201'
AND NOT EXISTS (SELECT 1 FROM portfolio.project WHERE project_title='Sample Library System');

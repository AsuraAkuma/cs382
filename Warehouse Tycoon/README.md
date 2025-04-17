# Warehouse Tycoon
**Description -** This game is about managing employees, departments, and warehouse flow. By shipping out loads of packages you earn money and can hire more employees, create more departments, upgrade empoyees/departments.

## Core Mechanics
- **Employees** - Can be hired, upgraded, transferred, sent home, and fired
    - Can also have individual traits that affect work performance
- **Departments** - Can be created, upgraded, managed, and destroyed
    - Random events can cause the department to temporarily or permanantly be non-operational
    - Can edit flow rate (package throughput), shutdown, edit employees assignment, edit manager assigned

## Game flow
1. Load into the game and sign in
2. Select a saved game to continue or create a new save
3. If a new game was created then run through tutorial

## Tutorial
1. Introduction - Welcome to Warehouse Tycoon, since it is your first day let me show you around.
2. UI - Showcase the ui and where everything is and what everything does

## UI Elements
- **Employee Manager** - Left side panel where you can view possible new hires and current employees
    - **New Hire Page** - Can hire new employees
    - **Current Employees Page** - Can move, send home, transfer departments, and fire
- **Department Manager** - Center panel where you can view all departments, manage each department, disable them, and destroy them
- **Upgrades** - Can purchase new upgrades for departments, employees, and the warehouse.

## Employee
### Core Stats

- **Speed** – How fast the employee moves items or travels between zones.
- **Efficiency** – How well they use time/resources (e.g. less downtime, fewer mistakes).
- **Stamina** – How long they can work before needing rest or a break.
- **Strength** – Affects how heavy of an item they can carry or how many items at once.
- **Focus** – Impacts accuracy and likelihood of making errors.
- **Experience** – Affects leveling up, promotions, or unlocking new roles.

### Department-Specific Stats

#### **HR (Human Resources)**  
- **Empathy** – Increases morale and reduces turnover by making employees feel valued. 
    - **Affected Stats** - (Stamina, Efficiency)  
- **Conflict Resolution** – Ability to handle disputes between employees, improving overall team cohesion. 
    - **Affected Stats** - (Efficiency, Focus)  
- **Recruiting** – Increases the likelihood of hiring skilled employees or finding specific talent. 
    - **Affected Stats** - (Experience, Efficiency)  

#### **IT (Information Technology)**  
- **Tech Troubleshooter** – Ability to fix software and hardware issues quickly, reducing downtime. 
    - **Affected Stats** - (Speed, Focus)  
- **System Optimization** – Enhances warehouse system efficiency, reducing errors in operations. 
    - **Affected Stats** - (Efficiency, Speed)  
- **Security** – Protects against cyber threats, ensuring smooth operations in tech-dependent departments. 
    - **Affected Stats** - (Focus, Stamina)  

#### **Operations**  
- **Logistics Planning** – Increases efficiency in workflow design and reduces travel time between departments. 
    - **Affected Stats** - (Efficiency, Speed)  
- **Task Management** – Organizes and prioritizes tasks to ensure everything runs smoothly. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Coordination** – Boosts teamwork and collaboration between departments. 
    - **Affected Stats** - (Efficiency, Stamina)  

#### **Inbound (Logistics)**  
- **Load Master** – Speeds up the unloading process and reduces mistakes in receiving shipments. 
    - **Affected Stats** - (Speed, Strength)  
- **Inventory Check** – Ensures accurate records of inbound shipments, reducing errors. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Speedy Unloader** – Increases the speed at which items are brought into the warehouse. 
    - **Affected Stats** - (Speed, Stamina)  

#### **Sorting**  
- **Sorting Speed** – Increases the speed of sorting items, allowing for quicker organization. 
    - **Affected Stats** - (Speed, Efficiency)  
- **Accuracy** – Reduces errors when sorting products, ensuring everything is in the right place. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Pattern Recognition** – Identifies the most efficient sorting patterns for quick organization. 
    - **Affected Stats** - (Efficiency, Speed)  

#### **Repacking**  
- **Packing Efficiency** – Makes the most out of available box space, reducing waste. 
    - **Affected Stats** - (Efficiency, Focus)  
- **Speed** – Increases the speed at which items are repacked. 
    - **Affected Stats** - (Speed, Stamina)  
- **Damage Control** – Reduces the risk of damaging items during repacking. 
    - **Affected Stats** - (Focus, Efficiency)  

#### **Palletizing**  
- **Pallet Efficiency** – Organizes items onto pallets in the most space-efficient way. 
    - **Affected Stats** - (Efficiency, Strength)  
- **Heavy Lifting** – Increases the speed and strength with which items are stacked. 
    - **Affected Stats** - (Strength, Speed)  
- **Stacking Precision** – Ensures that pallets are stacked securely, reducing breakage. 
    - **Affected Stats** - (Focus, Efficiency)  

#### **WaterSpidering**  
- **Route Efficiency** – Finds the fastest routes to deliver supplies and remove pallets. 
    - **Affected Stats** - (Speed, Efficiency)  
- **Carry Capacity** – Increases the number of items they can carry in one trip. 
    - **Affected Stats** - (Strength, Efficiency)  
- **Support Speed** – Quickly restocks supplies and provides assistance to other departments. 
    - **Affected Stats** - (Speed, Stamina)  

#### **FluidLoad**  
- **Loading Speed** – Increases the speed of loading items into trucks. 
    - **Affected Stats** - (Speed, Strength)  
- **Hard Hat Protection** – Reduces risk of accidents while loading, keeping workers safe. 
    - **Affected Stats** - (Stamina, Focus)  
- **Weight Distribution** – Helps distribute weight evenly when loading, reducing damage. 
    - **Affected Stats** - (Efficiency, Focus)  

#### **Quality Control**  
- **Attention to Detail** – Improves accuracy in finding defects and ensuring quality. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Inspection Speed** – Speeds up the inspection process without sacrificing accuracy. 
    - **Affected Stats** - (Speed, Focus)  
- **Product Knowledge** – Helps identify and handle specific types of products more efficiently. 
    - **Affected Stats** - (Experience, Efficiency)  

#### **Outbound (Logistics)**  
- **Shipping Accuracy** – Ensures that orders are shipped to the right destination without mistakes. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Load Efficiency** – Increases the efficiency of loading outbound shipments, reducing delays. 
    - **Affected Stats** - (Efficiency, Speed)  
- **Time Management** – Reduces shipping delays by prioritizing tasks. 
    - **Affected Stats** - (Efficiency, Speed)  

#### **Maintenance**  
- **Repair Speed** – Increases the speed of fixing machines or other equipment. 
    - **Affected Stats** - (Speed, Focus)  
- **Preventative Maintenance** – Reduces the likelihood of equipment failures by staying ahead of maintenance needs. 
    - **Affected Stats** - (Stamina, Efficiency)  
- **Tool Mastery** – Increases the effectiveness of the tools used for repairs. 
    - **Affected Stats** - (Efficiency, Experience)  

#### **Robotics**  
- **Robot Calibration** – Ensures robots operate at peak efficiency with minimal downtime. 
    - **Affected Stats** - (Efficiency, Focus)  
- **Speed Enhancement** – Increases the speed of robotic arms or machinery. 
    - **Affected Stats** - (Speed, Efficiency)  
- **Accuracy** – Reduces errors in the robot's movement, ensuring precise operations. 
    - **Affected Stats** - (Focus, Efficiency)  

#### **Safety**  
- **Hazard Identification** – Detects potential safety issues before they become a problem. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Accident Prevention** – Reduces the likelihood of accidents occurring in the warehouse. 
    - **Affected Stats** - (Stamina, Efficiency)  
- **Emergency Response** – Speeds up response times in the event of an emergency. 
    - **Affected Stats** - (Speed, Focus)  

#### **Cleaning**  
- **Speed** – Increases the speed of cleaning tasks around the warehouse. 
    - **Affected Stats** - (Speed, Stamina)  
- **Thoroughness** – Ensures all areas are cleaned properly, improving the warehouse environment. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Routine Maintenance** – Keeps the workspace clean and orderly, increasing overall efficiency. 
    - **Affected Stats** - (Efficiency, Stamina)  

#### **Security**  
- **Surveillance** – Increases the ability to monitor and secure the warehouse against theft. 
    - **Affected Stats** - (Focus, Efficiency)  
- **Alertness** – Improves response times to security breaches or suspicious activity. 
    - **Affected Stats** - (Speed, Focus)  
- **Patrol Speed** – Increases the speed of patrolling the warehouse to ensure safety. 
    - **Affected Stats** - (Speed, Efficiency)  

#### **Learning & Development**  
- **Training Effectiveness** – Improves the effectiveness of employee training programs. 
    - **Affected Stats** - (Experience, Efficiency)  
- **Skill Transfer** – Boosts the rate at which employees can gain new skills. 
    - **Affected Stats** - (Experience, Efficiency)  
- **Motivation** – Increases the likelihood that employees will complete training and improve their skills. 
    - **Affected Stats** - (Stamina, Experience)  

#### **Recruiting**  
- **Talent Scouting** – Increases the likelihood of finding high-quality candidates for open positions.  
    - **Affected Stats** - (Focus, Efficiency)  
- **Interviewing Skills** – Improves the ability to assess candidates accurately during interviews.  
    - **Affected Stats** - (Focus, Experience)  
- **Onboarding Efficiency** – Speeds up the process of integrating new hires into the company.  
    - **Affected Stats** - (Efficiency, Speed)  

### **Manager Department-Specific Stats**

#### **HR Manager**  
- **Morale Boost** – Raises overall employee happiness, decreasing turnover and burnout.  
  - **Affected Stats** – (Stamina, Efficiency)  
- **Retention Strategy** – Implements plans to keep skilled employees long-term.  
  - **Affected Stats** – (Experience, Focus)  
- **Policy Enforcement** – Maintains fairness and order across the workforce.  
  - **Affected Stats** – (Focus, Efficiency)  

#### **IT Manager**  
- **Infrastructure Oversight** – Ensures all systems are up-to-date and functional.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Incident Response** – Responds swiftly to tech outages or breaches.  
  - **Affected Stats** – (Speed, Focus)  
- **Tech Budgeting** – Allocates resources to maintain and upgrade systems effectively.  
  - **Affected Stats** – (Efficiency, Experience)  

#### **Operations Manager**  
- **Process Optimization** – Refines workflows to reduce bottlenecks and waste.  
  - **Affected Stats** – (Efficiency, Speed)  
- **Cross-Department Sync** – Improves coordination across all departments.  
  - **Affected Stats** – (Efficiency, Focus)  
- **KPI Monitoring** – Tracks performance metrics and adjusts for better output.  
  - **Affected Stats** – (Experience, Efficiency)  

#### **Inbound Manager**  
- **Dock Flow Management** – Coordinates truck schedules and dock availability.  
  - **Affected Stats** – (Speed, Efficiency)  
- **Receiving Accuracy** – Oversees item intake to prevent errors or losses.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Supplier Coordination** – Communicates effectively with suppliers to avoid delays.  
  - **Affected Stats** – (Experience, Efficiency)  

#### **Sorting Manager**  
- **Sort Line Oversight** – Ensures sorting stations are staffed and running optimally.  
  - **Affected Stats** – (Efficiency, Speed)  
- **Error Reduction Planning** – Introduces checks to catch and fix misroutes.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Peak Prep** – Prepares sorting teams for high-volume periods.  
  - **Affected Stats** – (Stamina, Efficiency)  

#### **Repacking Manager**  
- **Quality Check** – Oversees that all repacked items meet safety and presentation standards.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Material Allocation** – Ensures packing supplies are used effectively with minimal waste.  
  - **Affected Stats** – (Efficiency, Experience)  
- **Repack Flow** – Maintains a smooth flow of items through the repack area.  
  - **Affected Stats** – (Speed, Efficiency)  

#### **Palletizing Manager**  
- **Stacking Supervision** – Monitors pallet builds for space efficiency and safety.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Load Forecasting** – Predicts volume changes to ensure resource availability.  
  - **Affected Stats** – (Experience, Efficiency)  
- **Safety Checks** – Reduces risk of unstable loads or heavy-lift injuries.  
  - **Affected Stats** – (Stamina, Focus)  

#### **Waterspidering Manager**  
- **Route Planning** – Optimizes delivery and retrieval paths inside the warehouse.  
  - **Affected Stats** – (Speed, Efficiency)  
- **Support Coordination** – Anticipates which teams will need supplies next.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Load Distribution** – Balances workloads among waterspiders for maximum output.  
  - **Affected Stats** – (Efficiency, Stamina)  

#### **FluidLoad Manager**  
- **Truck Staging** – Plans and queues trucks for faster turnaround times.  
  - **Affected Stats** – (Speed, Efficiency)  
- **Team Synchronization** – Ensures fluidload team moves in harmony.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Loading Oversight** – Ensures each load is balanced and secure.  
  - **Affected Stats** – (Focus, Strength)  

#### **Quality Control Manager**  
- **Inspection Protocols** – Establishes inspection standards for product quality.  
  - **Affected Stats** – (Focus, Experience)  
- **Defect Reporting** – Ensures consistent tracking and communication of quality issues.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Continuous Improvement** – Develops methods to improve product standards over time.  
  - **Affected Stats** – (Experience, Efficiency)  

#### **Outbound Manager**  
- **Load Scheduling** – Coordinates outbound shipments to meet deadlines.  
  - **Affected Stats** – (Efficiency, Speed)  
- **Accuracy Oversight** – Verifies all orders match customer and shipping data.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Carrier Coordination** – Communicates with transport teams to reduce delays.  
  - **Affected Stats** – (Experience, Efficiency)  

#### **Maintenance Manager**  
- **Repair Workflow** – Assigns and monitors repairs across all zones.  
  - **Affected Stats** – (Efficiency, Speed)  
- **Part Inventory** – Keeps track of tools and replacement parts.  
  - **Affected Stats** – (Focus, Experience)  
- **Maintenance Scheduling** – Plans preventative checks to avoid breakdowns.  
  - **Affected Stats** – (Stamina, Efficiency)  

#### **Robotics Manager**  
- **Automation Planning** – Strategizes how robotics can streamline tasks.  
  - **Affected Stats** – (Efficiency, Experience)  
- **Firmware Management** – Keeps software and firmware updated across all robotics.  
  - **Affected Stats** – (Focus, Efficiency)  
- **Robot Uptime** – Ensures all machines stay active and functioning.  
  - **Affected Stats** – (Speed, Efficiency)  

#### **Safety Manager**  
- **Audit Execution** – Conducts thorough safety inspections.  
  - **Affected Stats** – (Focus, Experience)  
- **Training Enforcement** – Makes sure staff complete safety training.  
  - **Affected Stats** – (Experience, Efficiency)  
- **Incident Review** – Quickly analyzes and responds to safety incidents.  
  - **Affected Stats** – (Focus, Speed)  

#### **Cleaning Manager**  
- **Zone Prioritization** – Assigns cleaners based on traffic and needs.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Supply Management** – Keeps cleaning tools and supplies organized and stocked.  
  - **Affected Stats** – (Experience, Efficiency)  
- **Cleanliness Standards** – Maintains consistent quality across all zones.  
  - **Affected Stats** – (Focus, Stamina)  

#### **Security Manager**  
- **Surveillance Oversight** – Ensures all cameras and alarm systems are monitored.  
  - **Affected Stats** – (Focus, Experience)  
- **Patrol Routing** – Plans the most efficient patrol paths.  
  - **Affected Stats** – (Speed, Efficiency)  
- **Threat Assessment** – Evaluates risks and makes adjustments to reduce vulnerabilities.  
  - **Affected Stats** – (Focus, Efficiency)  

#### **Learning & Development Manager**  
- **Curriculum Design** – Creates training plans tailored to each role.  
  - **Affected Stats** – (Experience, Efficiency)  
- **Progress Tracking** – Monitors employee learning milestones.  
  - **Affected Stats** – (Focus, Experience)  
- **Upskilling Strategy** – Identifies and implements skill growth opportunities.  
  - **Affected Stats** – (Efficiency, Experience)  

#### **Recruiting Manager**  
- **Candidate Pipeline Management** – Ensures a steady flow of qualified candidates for open positions.  
  - **Affected Stats** – (Efficiency, Focus)  
- **Interview Oversight** – Improves the accuracy and effectiveness of the interview process.  
  - **Affected Stats** – (Focus, Experience)  
- **Onboarding Strategy** – Optimizes the onboarding process to integrate new hires quickly and effectively.  
  - **Affected Stats** – (Efficiency, Speed)  

### Traits / Modifiers  

#### **HR (Human Resources)**  
- **Empathetic Listener** – Increases employee morale in nearby departments, reducing turnover.  
    - **Primary Stat Bonus**: +10% Stamina (Reduces fatigue from high turnover)  
- **Policy Enforcer** – Reduces workplace conflicts, increasing overall department efficiency.  
    - **Primary Stat Bonus**: +15% Efficiency (Smoother workflow)  
- **Employee Advocate** – Employees are less likely to quit and perform better under stress.  
    - **Primary Stat Bonus**: +10% Focus (Better performance under pressure)  

#### **IT (Information Technology)**  
- **Tech Savvy** – Fixes technical issues faster, reducing downtime.  
    - **Primary Stat Bonus**: +20% Speed (Faster issue resolution)  
- **Cyber Guardian** – Reduces the risk of system failures or cyber attacks.  
    - **Primary Stat Bonus**: +15% Efficiency (Prevents disruptions)  
- **Automation Expert** – Increases efficiency of warehouse systems and automation, saving time.  
    - **Primary Stat Bonus**: +10% Speed, +10% Efficiency (Optimizes automation)  

#### **Operations**  
- **Task Master** – Increases task completion speed across all departments.  
    - **Primary Stat Bonus**: +15% Speed (Faster task handling)  
- **Organized Leader** – Boosts morale and performance of employees under supervision.  
    - **Primary Stat Bonus**: +10% Efficiency, +5% Stamina (Better morale = less fatigue)  
- **Problem Solver** – Reduces downtime caused by operational issues or miscommunications.  
    - **Primary Stat Bonus**: +10% Focus (Reduces errors)  

#### **Inbound (Logistics)**  
- **Quick Unloader** – Increases speed when unloading shipments from trucks.  
    - **Primary Stat Bonus**: +20% Speed (Faster unloading)  
- **Inventory Genius** – Reduces errors when logging and receiving items.  
    - **Primary Stat Bonus**: +15% Focus (Better accuracy)  
- **Cargo Expert** – Improves the accuracy of inventory received, reducing mistakes.  
    - **Primary Stat Bonus**: +10% Efficiency (Smoother receiving process)  

#### **Sorting**
- **Fast Sorter** – Increases sorting speed, reducing time spent in the sorting area.
    - **Primary Stat Bonus**: +20% Speed (Faster processing)
- **Accuracy Guru** – Less likely to misplace items, improving overall accuracy in sorting.
    - **Primary Stat Bonus**: +15% Focus (Fewer errors)
- **Pattern Finder** – Boosts sorting efficiency by optimizing item flow and arrangement.
    - **Primary Stat Bonus**: +10% Efficiency (Better organization)

#### **Repacking**
- **Efficient Packer** – Packs boxes quickly and efficiently, using less material.
    - **Primary Stat Bonus**: +15% Speed, +5% Efficiency (Faster packing with less waste)
- **Quality Packager** – Ensures items are packed securely, reducing product damage.
    - **Primary Stat Bonus**: +10% Focus (Better packaging quality)
- **Space Saver** – Maximizes available space in boxes, reducing packaging waste.
    - **Primary Stat Bonus**: +10% Efficiency (Optimized space usage)

#### **Palletizing**
- **Heavy Lifter** – Increases strength, allowing for quicker palletizing of heavy goods.
    - **Primary Stat Bonus**: +20% Strength (Handles heavier loads)
- **Stack Master** – Organizes items onto pallets in the most space-efficient way.
    - **Primary Stat Bonus**: +15% Efficiency (Better space utilization)
- **Quick Stacker** – Increases speed in stacking, reducing pallet preparation time.
    - **Primary Stat Bonus**: +10% Speed (Faster pallet building)

#### **WaterSpidering**
- **Swift Runner** – Increases speed of delivering supplies across the warehouse.
    - **Primary Stat Bonus**: +20% Speed (Faster deliveries)
- **Multi-Tasker** – Can carry multiple items at once, improving efficiency.
    - **Primary Stat Bonus**: +15% Efficiency (More items handled simultaneously)
- **Quick Responder** – Delivers necessary items to departments faster, reducing downtime.
    - **Primary Stat Bonus**: +10% Speed, +5% Focus (Fast response with fewer mistakes)

#### **FluidLoad**
- **Load Master** – Increases the speed and efficiency of loading items onto trucks.
    - **Primary Stat Bonus**: +15% Speed, +5% Strength (Faster loading of heavy items)
- **Safety-First** – Reduces injury risk, maintaining higher productivity over time.
    - **Primary Stat Bonus**: +10% Stamina (Fewer injuries = longer work periods)
- **Weight Distributor** – Ensures even weight distribution, reducing shipping errors.
    - **Primary Stat Bonus**: +10% Focus (Better load balancing)

#### **Quality Control**
- **Eagle Eye** – Increases the likelihood of spotting defects or quality issues.
    - **Primary Stat Bonus**: +20% Focus (Better defect detection)
- **Thorough Inspector** – Performs quality checks faster without missing details.
    - **Primary Stat Bonus**: +15% Speed (Faster inspections)
- **Perfectionist** – Less likely to miss defects, but slightly slower in performing inspections.
    - **Primary Stat Bonus**: +25% Focus, -5% Speed (More accurate but slower)

#### **Outbound (Logistics)**
- **Route Planner** – Optimizes routes for faster delivery and reduced shipping costs.
    - **Primary Stat Bonus**: +15% Efficiency (Better route optimization)
- **On-Time Shipper** – Increases the likelihood of meeting shipping deadlines.
    - **Primary Stat Bonus**: +10% Speed (Faster processing)
- **Accurate Shipper** – Reduces shipping errors, ensuring customers receive the right items.
    - **Primary Stat Bonus**: +10% Focus (Fewer shipping mistakes)

#### **Maintenance**
- **Fix-It Fast** – Repairs equipment and machinery faster, reducing downtime.
    - **Primary Stat Bonus**: +20% Speed (Quicker repairs)
- **Tool Master** – Reduces tool wear, increasing repair efficiency.
    - **Primary Stat Bonus**: +15% Efficiency (Better tool usage)
- **Preventive Pro** – Reduces the likelihood of machine breakdowns with regular maintenance.
    - **Primary Stat Bonus**: +10% Stamina (Longer machine lifespan)

#### **Robotics**
- **Calibrated Precision** – Increases the accuracy of robots in performing tasks.
    - **Primary Stat Bonus**: +15% Focus (More precise operations)
- **Robot Tuner** – Increases robotic efficiency, allowing them to work faster and with fewer errors.
    - **Primary Stat Bonus**: +10% Speed, +5% Efficiency (Optimized robot performance)
- **Mechanical Wizard** – Increases robot durability, reducing the need for frequent repairs.
    - **Primary Stat Bonus**: +10% Stamina (Longer robot uptime)

#### **Safety**
- **Alert** – Detects potential hazards and prevents accidents before they happen.
    - **Primary Stat Bonus**: +15% Focus (Better hazard detection)
- **Calm Under Pressure** – Reduces panic during safety incidents, keeping morale steady.
    - **Primary Stat Bonus**: +10% Stamina (Better crisis management)
- **Safety Guru** – Improves overall safety compliance in the warehouse, reducing accidents.
    - **Primary Stat Bonus**: +10% Efficiency (Smoother safety operations)

#### **Cleaning**
- **Speedy Scrubber** – Cleans areas faster, reducing downtime during cleaning shifts.
    - **Primary Stat Bonus**: +20% Speed (Faster cleaning)
- **Deep Clean** – Provides thorough cleaning that reduces warehouse hazards or product defects.
    - **Primary Stat Bonus**: +15% Focus (More thorough cleaning)
- **Routine Master** – Increases efficiency when cleaning according to the schedule, ensuring all areas are maintained.
    - **Primary Stat Bonus**: +10% Efficiency (Better schedule adherence)

#### **Security**
- **Watchful Eye** – Increases surveillance accuracy, spotting potential threats earlier.
    - **Primary Stat Bonus**: +15% Focus (Better threat detection)
- **Quick Responder** – Responds faster to security breaches or suspicious activity.
    - **Primary Stat Bonus**: +10% Speed (Faster response times)
- **Patrol Efficiency** – Reduces time spent on patrols while still covering all areas.
    - **Primary Stat Bonus**: +10% Efficiency (Optimized patrol routes)

#### **Learning & Development**
- **Training Expert** – Speeds up the rate at which employees learn new skills.
    - **Primary Stat Bonus**: +15% Experience (Faster skill acquisition)
- **Skill Transfer** – Passively boosts the skills of employees nearby through mentoring.
    - **Primary Stat Bonus**: +10% Efficiency (Better knowledge sharing)
- **Motivational Speaker** – Increases employee engagement in training, leading to faster progress.
    - **Primary Stat Bonus**: +10% Stamina (Higher training endurance)

#### **Recruiting**  
- **Talent Magnet** – Increases the likelihood of attracting high-quality candidates for open positions.  
    - **Primary Stat Bonus**: +15% Focus (Better candidate identification)  
- **Charismatic Interviewer** – Improves the success rate of interviews, ensuring better hires.  
    - **Primary Stat Bonus**: +10% Experience (More accurate candidate evaluation)  
- **Efficient Onboarder** – Speeds up the onboarding process, reducing downtime for new hires.  
    - **Primary Stat Bonus**: +10% Efficiency (Faster integration of new employees)  

### Department Specific Actions

#### **HR Manager**
**Primary Task:**  
- **Talent Acquisition** – Analyze warehouse needs, review new hire applicants, hire the most suitable applicant.

**Secondary Task:**  
- **Resolve Employee Conflicts** – Occasionally, random conflict events appear between employees. The HR Manager must choose how to handle them (e.g. mediation, warnings, transfers), impacting morale and productivity.

#### **HR Employee**
**Primary Task:**  
- **Handle Tickets** – Recieve new tickets from employees, identify the issue they are having, and resolve the issue.

**Secondary Task:**  
- **Review Old Tickets** – Look into old tickets to gain experience.

#### **IT Manager**  
**Primary Task:**  
- **System Monitoring** – Oversee critical systems and software used in the warehouse. Detect and resolve errors to avoid workflow disruption.

**Secondary Task:**  
- **Approve Tech Requests** – Review and approve or deny IT hardware/software requests submitted by employees.

#### **IT Employee**  
**Primary Task:**  
- **Fix Technical Issues** – Respond to reported technical issues such as malfunctioning devices, slow network, or software errors.

**Secondary Task:**  
- **Run Diagnostics** – Perform regular diagnostics on equipment and systems to prevent future issues.

#### **Operations Manager**  
**Primary Task:**  
- **Oversee Department Performance** – Monitor overall workflow and performance metrics across departments. Reassign resources if needed.

**Secondary Task:**  
- **Implement Process Improvements** – Analyze bottlenecks and suggest workflow optimizations to improve warehouse efficiency.

#### **Operations Employee**  
**Primary Task:**  
- **Assist with Scheduling** – Help create shift schedules, accounting for employee availability and workload demands.

**Secondary Task:**  
- **Update Performance Logs** – Record departmental metrics for analysis and reporting.

#### **Inbound Manager**  
**Primary Task:**  
- **Coordinate Inbound Shipments** – Schedule and receive incoming shipments. Ensure materials are scanned and stored properly.

**Secondary Task:**  
- **Vendor Communication** – Communicate with suppliers to address delivery delays or damaged goods.

#### **Inbound Employee**  
**Primary Task:**  
- **Unload Shipments** – Receive and unload materials, verifying the contents against delivery records.

**Secondary Task:**  
- **Label and Sort Items** – Label incoming items and sort them by destination within the warehouse.

#### **Sorting Manager**  
**Primary Task:**  
- **Optimize Sorting Layout** – Design and adjust sorting lines to minimize movement and time spent handling items.

**Secondary Task:**  
- **Audit Sort Accuracy** – Randomly check sorted bins for errors and retrain staff if needed.

#### **Sorting Employee**  
**Primary Task:**  
- **Sort Packages** – Use scanning tools to sort packages into their designated areas.

**Secondary Task:**  
- **Clear Backlogs** – Work overtime or in shifts to handle unexpected spikes in incoming items.

#### **Repacking Manager**  
**Primary Task:**  
- **Manage Packaging Supplies** – Ensure an adequate supply of boxes, fillers, and labels are available for repacking operations.

**Secondary Task:**  
- **Quality Check Repacked Items** – Inspect repacked items for damage or improper packing.

#### **Repacking Employee**  
**Primary Task:**  
- **Repack Damaged or Mixed Items** – Open and repackage items that were damaged or incorrectly packed.

**Secondary Task:**  
- **Weigh and Label Boxes** – Ensure accurate weight and shipping labels are applied to repacked goods.

#### **Palletizing Manager**  
**Primary Task:**  
- **Plan Pallet Configurations** – Define how boxes are stacked on pallets to optimize space and stability.

**Secondary Task:**  
- **Check Load Balance** – Ensure that each pallet meets weight and safety standards before shipment.

#### **Palletizing Employee**  
**Primary Task:**  
- **Load Boxes onto Pallets** – Physically stack and secure boxes on pallets using strapping tools.

**Secondary Task:**  
- **Move Finished Pallets** – Transport finished pallets to staging areas using pallet jacks.

#### **Water Spidering Manager**  
**Primary Task:**  
- **Create Delivery Routes** – Plan efficient supply routes for water spiders to reduce downtime in production areas.

**Secondary Task:**  
- **Inventory Checks** – Oversee supply inventory to ensure essential materials are always available.

#### **Water Spidering Employee**  
**Primary Task:**  
- **Deliver Supplies** – Bring needed materials (tape, boxes, equipment) to workers on the floor.

**Secondary Task:**  
- **Remove Full Pallets** – Take full pallets from workstations to the appropriate outbound or staging area.

#### **Fluid Load Manager**  
**Primary Task:**  
- **Schedule Load Crews** – Assign load crews to trucks and docks, ensuring manpower is evenly distributed.

**Secondary Task:**  
- **Inspect Loading Zones** – Monitor safety and efficiency in active truck loading zones.

#### **Fluid Load Employee**  
**Primary Task:**  
- **Load Trucks Manually** – Carefully stack boxes by hand into trucks wearing safety gear.

**Secondary Task:**  
- **Secure Freight** – Use straps and spacers to secure loads and prevent shifting during transit.

#### **Quality Control Manager**  
**Primary Task:**  
- **Define QC Protocols** – Set standards and procedures for inspecting items throughout the warehouse.

**Secondary Task:**  
- **Handle Defect Reports** – Review reports of defective products and decide on disposal or return.

#### **Quality Control Employee**  
**Primary Task:**  
- **Inspect Products** – Check items at various stages for damage, expiration, or mislabeling.

**Secondary Task:**  
- **Document Issues** – Record product defects in the QC system for future audits.

#### **Outbound Manager**  
**Primary Task:**  
- **Oversee Shipment Accuracy** – Ensure orders leaving the warehouse match packing slips and are on schedule.

**Secondary Task:**  
- **Coordinate with Carriers** – Communicate with shipping companies for pickups and special delivery instructions.

#### **Outbound Employee**  
**Primary Task:**  
- **Scan and Ship Orders** – Final check and scanning of boxes before loading onto trucks.

**Secondary Task:**  
- **Organize Staging Area** – Group outgoing shipments for efficient loading and prevent mix-ups.

#### **Maintenance Manager**  
**Primary Task:**  
- **Plan Maintenance Schedule** – Organize preventative maintenance routines for all equipment.

**Secondary Task:**  
- **Order Repair Parts** – Keep track of parts inventory and place orders as needed.

#### **Maintenance Employee**  
**Primary Task:**  
- **Fix Equipment** – Perform repairs on forklifts, conveyors, and other warehouse machinery.

**Secondary Task:**  
- **Emergency Response** – Respond to sudden equipment failures during operations.

#### **Robotics Manager**  
**Primary Task:**  
- **Monitor Robotics System** – Oversee robot arm functionality and workflow integrations.

**Secondary Task:**  
- **Schedule Updates** – Apply firmware or software updates to robotic systems.

#### **Robotics Employee**  
**Primary Task:**  
- **Calibrate Robots** – Adjust robot arms for precision stacking and movement.

**Secondary Task:**  
- **Clean Robot Sensors** – Keep vision systems and sensors clean for accurate operation.

#### **Safety Manager**  
**Primary Task:**  
- **Conduct Safety Inspections** – Walk through the warehouse checking for hazards or violations.

**Secondary Task:**  
- **Organize Drills** – Schedule and lead emergency drills (e.g. fire, spill, earthquake).

#### **Safety Employee**  
**Primary Task:**  
- **Log Safety Incidents** – Record accidents, near misses, and unsafe behavior.

**Secondary Task:**  
- **Distribute Safety Gear** – Hand out and monitor use of gloves, helmets, goggles, etc.

#### **Cleaning Manager**  
**Primary Task:**  
- **Plan Cleaning Rotations** – Assign cleaning staff to zones and monitor cleaning frequency.

**Secondary Task:**  
- **Inspect Cleanliness Levels** – Perform checks to ensure hygiene standards are maintained.

#### **Cleaning Employee**  
**Primary Task:**  
- **Clean Designated Zones** – Sweep, mop, and sanitize areas including breakrooms and work floors.

**Secondary Task:**  
- **Restock Supplies** – Refill soap, paper towels, and cleaning materials.

#### **Security Manager**  
**Primary Task:**  
- **Oversee Surveillance** – Monitor security camera footage and investigate suspicious behavior.

**Secondary Task:**  
- **Set Security Protocols** – Define procedures for access control, lockdowns, and intrusions.

#### **Security Employee**  
**Primary Task:**  
- **Perform Patrols** – Regularly check entry points, restricted areas, and exits.

**Secondary Task:**  
- **Check Badges** – Ensure all personnel have proper ID and restrict unauthorized access.

#### **Learning and Development Manager**  
**Primary Task:**  
- **Design Training Programs** – Develop modules for onboarding, promotions, and cross-training.

**Secondary Task:**  
- **Evaluate Trainee Progress** – Track performance of employees undergoing training.

#### **Learning and Development Employee**  
**Primary Task:**  
- **Host Training Sessions** – Conduct sessions on equipment use, safety, or company policy.

**Secondary Task:**  
- **Assist with Onboarding** – Help new hires complete orientation and paperwork.

#### **Recruiting**  
**Primary Task:**  
- **New Hire Search** – Search for, create, new hires adding them to the newHire list.

**Secondary Task:**  
- **Career Fair** – Attend career fairs to find hot talent.

# Notes
## Notifications
- Create notification function and queue
- Add notification calls to functions when something happens

# UI Colors
- Gray - #2E2E2E
- DarkGray - #1F1F1F
- Green - #3BFF00
- Orange - #FF8D00
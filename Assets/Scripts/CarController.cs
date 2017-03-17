using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Collider C_car;
    private Rigidbody Car;
    private AudioSource A_car;
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.

    
    public float Speed;
    public float Boost;
    public float TurnSpeed;
    public float JumpH;
    public float startingPitch;
    public float Gravity;
    public float Frein;
    public float Flip;
    public float tolerance=100;
    public bool sol;
    private bool BUMP;

    private GamepadManager manager;
    public SoundManager A_manager;
    private float distToGround;
    private float decalage;
    private float angle;
    private Vector3 direction;
    private bool auSol;
    private float pitch;
    public ParticleSystem   Smoke;
    public ParticleSystem   Turbo;
    public Collider boost;
    private bool boostOn;
    public x360_Gamepad gamepad;
    //private Quaternion bonneOrientation;
    public int papa;

    private float flipAmount;
    private float flipTampon;

    private void Awake()
    {
        Car = GetComponent<Rigidbody>();
        

    }
    void Start()
    {
       A_manager.Source.Add(gameObject.GetComponent<AudioSource>());
       
        A_car = GetComponent<AudioSource>();
        
        manager = GamepadManager.Instance;
        gamepad = manager.GetGamepad(papa);

        pitch = startingPitch + Random.Range(-0.1f, 0.1f);
        Smoke.Pause();
        distToGround = C_car.bounds.extents.y+1;
        //Debug.Log(distToGround);
        Turbo.Pause();
        //bonneOrientation = transform.rotation;
    }
    void Update()
    {
        //Debug.Log(Physics.Raycast(transform.position, -Vector3.up, (distToGround + tolerance) * 10));
       // Debug.Log(Car.transform.localEulerAngles.x);
        Debug.Log(Car.transform.localEulerAngles.z);
        //fail();
        //Debug.Log(Car.velocity.magnitude);
        //Debug.Log(boostOn);
        //float Lx = gamepad.GetStick_L().X;
        // Debug.Log(gamepad.GetStick_L().X);
        IsGrounded();

        //Debug.Log(Car.velocity);
        //   decalage = new Vector3(0, Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI, 0);
        angle = transform.localEulerAngles.y;
        angle = (angle > 180) ? angle - 360 : angle;
        // Debug.Log("voiture" + angle);
        // Debug.Log("stick" + Mathf.Atan2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal")) * 180 / Mathf.PI);

        if (auSol && gamepad.GetButton("A") && (gamepad.GetStick_L().Y < 0.5 && gamepad.GetStick_L().Y > -0.5 && gamepad.GetStick_L().X > -0.5 && gamepad.GetStick_L().X < 0.5) && !gamepad.GetButton("B"))
        {

            Move();
            /*
            if(Turbo.isStopped)
            {
                Turbo.Play();
            }
            */
        }

        if ((gamepad.GetStick_L().Y > 0.5 || gamepad.GetStick_L().Y < -0.5 || gamepad.GetStick_L().X < -0.5 || gamepad.GetStick_L().X > 0.5) && auSol)
        {
            direction = new Vector3(0, Mathf.Atan2(gamepad.GetStick_L().Y, -gamepad.GetStick_L().X) * 180 / Mathf.PI, 0);
            Turn();
            if (gamepad.GetButton("B") == false)
            {
                Move();
            }
        }

        else
        {
            // Debug.Log(Car.velocity);
        }

            if (auSol == false)
            {
                Car.velocity = new Vector3(Car.velocity.x, Car.velocity.y - 0.05f, Car.velocity.z);
                Car.AddForce(Car.velocity * 2f);
            /*
            if (Car.velocity.magnitude > 5 || (Car.transform.eulerAngles.x))
            {
                Car.velocity = new Vector3(Car.velocity.x, Car.velocity.y - 0.05f, Car.velocity.z);
                Car.AddForce(Car.velocity * 2f);

                Vector3 _good = new Vector3(0, Car.transform.rotation.y, 0);
                Vector3 _bad = new Vector3(Car.transform.rotation.x, Car.transform.rotation.y, Car.transform.rotation.z);



                Vector3 x = Vector3.Cross(_bad.normalized, _good.normalized);
                float theta = Mathf.Asin(x.magnitude);
                Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
                Quaternion q = transform.rotation * Car.inertiaTensorRotation;
                Vector3 T = q * Vector3.Scale(Car.inertiaTensor, (Quaternion.Inverse(q) * w));
                Car.AddTorque(T / 1000, ForceMode.Force);

            }
            */
            /*
            if (Car.velocity.magnitude < 5)
            {
            */
            if (sol && Physics.Raycast(transform.position, -Vector3.up, (distToGround + tolerance)*10))
            {
                if (Car.transform.localEulerAngles.x < -80 && Car.transform.localEulerAngles.x > -100)
                {
                    Debug.Log("arriere");
                    Car.AddRelativeTorque(new Vector3(40, 0, 0), ForceMode.VelocityChange);
                }
                if (Car.transform.localEulerAngles.x > 80 && Car.transform.localEulerAngles.x < 100)
                {
                    Debug.Log("arriere");
                    Car.AddRelativeTorque(new Vector3(-40, 0, 0), ForceMode.VelocityChange);
                }
                if(Car.transform.localEulerAngles.z>80&& Car.transform.localEulerAngles.z<180)
                {
                    Car.AddRelativeTorque(new Vector3(0, 0, -40), ForceMode.VelocityChange);
                }
                if (Car.transform.localEulerAngles.z  <300 && Car.transform.localEulerAngles.z>=180)
                {
                    Car.AddRelativeTorque(new Vector3(0, 0, 40), ForceMode.VelocityChange);
                }
            }
                /*
                //(!auSol)&&fail()
                if (Car.velocity.magnitude < 5)
                {
                    Vector3 _good = new Vector3(0, Car.transform.rotation.y, 0);
                    Vector3 _bad = new Vector3(Car.transform.rotation.x, Car.transform.rotation.y, Car.transform.rotation.z);



                    Vector3 x = Vector3.Cross(_bad.normalized, _good.normalized);
                    float theta = Mathf.Asin(x.magnitude);
                    Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
                    Quaternion q = transform.rotation * Car.inertiaTensorRotation;
                    Vector3 T = q * Vector3.Scale(Car.inertiaTensor, (Quaternion.Inverse(q) * w));
                    Car.AddTorque(T / 75, ForceMode.VelocityChange);
                }
                */
                /*
                if (Car.transform.rotation.z > 50)
                {
                    Car.AddRelativeTorque(new Vector3(0, 0, -40), ForceMode.VelocityChange);
                }
                if(Car.transform.rotation.z<-50)
                {
                    Car.AddRelativeTorque(new Vector3(0, 0,40), ForceMode.VelocityChange);
                }
                */
                //  transform.rotation = Quaternion.Slerp(transform.rotation, bonneOrientation, 0.1f * Time.deltaTime);
            
                //StartCoroutine(up());
            

            // float step = TurnSpeed * Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation, step/10);
            /*
                        while (Car.velocity.magnitude>0.1)
                        {
                            Vector3 movement = Car.velocity * 0.5f ;

                            Car.velocity = movement;
                            //Car.AddForce(movement);
                        }

                    */

            //  Vector3 movement = Vector3.Lerp(Car.velocity, Vector3.zero, Frein);
            //    Car.velocity = movement;
            // Vector3 movement = Vector3.SmoothDamp(Car.velocity, Vector3.zero, ref currentV, Time.deltaTime);
            //Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            Smoke.Stop();
            if ((!gamepad.GetButton("A")) || !auSol)
            {
                Turbo.Stop();
            }

        }
        if (gamepad.GetButton("B") && auSol)
        {
            marcheArriere();
        }
        if (gamepad.GetButton("X"))
        {
            //Debug.Log("yep");
            BarrelRoll();
            //GetUp();
        }
        if (gamepad.GetButton("L3"))
        {
            //GetUp();
            Jump();
        }

        if (gamepad.GetButtonDown("Y"))
        {
            Horn();
        }

        m_MovementInputValue = gamepad.GetStick_L().Y;
        m_TurnInputValue = gamepad.GetStick_L().X;

        // OldTurn();

        if (sol)
        {

            if (Car.transform.eulerAngles.x > 80 && Car.transform.eulerAngles.x < 100)
            {
                //Debug.Log("DH ADSHGAU");
                Car.AddRelativeTorque(new Vector3(-20, 0, 0), ForceMode.VelocityChange);

            }
            if (Car.transform.eulerAngles.x >= 180 && Car.transform.eulerAngles.x < 290)
            {

                Car.AddRelativeTorque(new Vector3(20, 0, 0), ForceMode.VelocityChange);
            }
            if (Car.transform.eulerAngles.z > 70 && Car.transform.eulerAngles.z < 180)
            {
                Debug.Log("oi");
                Car.AddRelativeTorque(new Vector3(0, 0, -20), ForceMode.VelocityChange);
            }

        }
    }
    
    /*
    IEnumerator getUp()
    {
        Vector3 _good = new Vector3(0, Car.transform.rotation.y, 0);
        Vector3 _bad = new Vector3(Car.transform.rotation.x, Car.transform.rotation.y, Car.transform.rotation.z);



        Vector3 x = Vector3.Cross(_bad.normalized, _good.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
        Quaternion q = transform.rotation * Car.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(Car.inertiaTensor, (Quaternion.Inverse(q) * w));
        Car.AddTorque(T / 100, ForceMode.VelocityChange);

        yield return new WaitUntil(auSol);
    }
    */
    private void Turn()
    {

        float step = TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, direction.y, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, step);
        // transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal")) * 180 / Mathf.PI, 0);
        // if ()

    }
   

    private void FixedUpdate()
    {
        if(!auSol)
        {
            //flipAmount += flipTampon - transform.rotation.x;
        }


        // Move();
        /*Debug.Log(Car.rotation.z);
                  if((Car.rotation.z>0.65f ||Car.rotation.z<-0.65)&& auSol)
                {
                    GetUp();
                }
                */
        //Run();
        if (BUMP)
        {
            Jump();
            
            //Debug.Log("oi");
            /*
            Car.AddForce(new Vector3(0,Car.velocity.magnitude), ForceMode.Impulse);
            BUMP = false;
            */
        }
    }
    private void marcheArriere()
    {
        float _hauteur = Car.velocity.y * Time.deltaTime;
        Vector3 movement = transform.forward * -Speed * Time.deltaTime;
        movement = -movement;

        movement.y = _hauteur;
        if (gamepad.GetButton("A"))
        {
            movement = movement * Boost;
            Turbo.Play();

            Smoke.Stop();

        }
        else
        {
            Smoke.Play();
            Turbo.Stop();

        }
        if (boostOn == true)
        {
            movement = movement * 3.5f;
        }
        Car.AddForce(movement, ForceMode.Impulse);
    }
    private void Move()
    {
        float _hauteur = Car.velocity.y*Time.deltaTime;
        Vector3 movement = transform.forward  * -Speed *Time.deltaTime;
        /*
        if (gamepad.GetButton("B"))
        {
            movement = -movement/2;
        }
        */
        movement.y = _hauteur;
        //Debug.Log(movement.y);
        if (gamepad.GetButton("A"))
        {
            movement = movement * Boost;
            Turbo.Play();
            
            Smoke.Stop();
        }
        else
        {
            Smoke.Play();
            Turbo.Stop();
            
        }
        
        if(boostOn==true)
        {
            movement = movement * 3.5f;
        }

        // Car.velocity =  movement;
        Car.AddForce(movement, ForceMode.Impulse);


        /*

        Vector3 movement = transform.forward * m_MovementInputValue * Speed * Time.deltaTime;
        
       if (Input.GetButton("Fire1"))
        {
            movement = movement * Boost;
        }
        // Apply this movement to the rigidbody's position.
       // Debug.Log("on" + movement);
        Car.MovePosition(Car.position + movement);
        */
    }
    private void OldTurn()
    {

        /*   if (Input.GetButton("Horizontal") && m_TurnInputValue >0.5)
           {

               float turn = m_TurnInputValue * TurnSpeed * Time.deltaTime  ;
               Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

               Car.MoveRotation( Car.rotation * turnRotation );
           }
           */
           
        if (Input.GetButton("Horizontal") && m_TurnInputValue > 0.5 && angle > -90  && angle <= 90)

        {
            
            transform.Rotate(0, TurnSpeed, 0);
                }

        if (Input.GetButton("Horizontal") && m_TurnInputValue > 0.5 && ((angle<-90)||(angle>90)))

        {

            transform.Rotate(0, -TurnSpeed, 0);
        }


        if (Input.GetButton("Horizontal") && m_TurnInputValue < -0.5 && angle < 90 && angle >= -90)

        {

            transform.Rotate(0, -TurnSpeed, 0);
        }


        if (Input.GetButton("Horizontal") && m_TurnInputValue < -0.5 && ((angle < -90) || (angle > 90)))

        {

            transform.Rotate(0, TurnSpeed, 0);
        }
        
        if(Input.GetButton("Vertical") && m_MovementInputValue >0.5 && angle>=0 && angle<=180)
        {
            transform.Rotate(0, TurnSpeed, 0);
        }

        if (Input.GetButton("Vertical") && m_MovementInputValue > 0.5 && angle < 0 && angle > -180)
        {
            transform.Rotate(0, -TurnSpeed, 0);
        }

        if (Input.GetButton("Vertical") && m_MovementInputValue < -0.5 && angle <= 0 && angle >= -180)
        {
            transform.Rotate(0, TurnSpeed, 0);
        }

        if (Input.GetButton("Vertical") && m_MovementInputValue < -0.5 && angle > 0 && angle < 180)
        {
            transform.Rotate(0, -TurnSpeed, 0);
        }

        /*if (Input.GetButton("Horizontal") && m_TurnInputValue < -0.5)
        {
            Debug.Log(m_TurnInputValue);
            float turn = m_TurnInputValue * TurnSpeed* Time.deltaTime ;
            Quaternion turnRotation = Quaternion.Euler(0f, turn , 0f);
            Car.MoveRotation(turnRotation);
        }
        */
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        // float turn = m_TurnInputValue * TurnSpeed * Time.deltaTime;
        /* Vector3 movement = transform.forward * -Speed * Time.deltaTime;
         if (Input.GetButton("Horizontal")&& Input.GetButton("Vertical")==false)
         {
             Car.MovePosition(Car.position + movement);
             //Debug.Log("turn" + movement);
         }*/

        // Make this into a rotation in the y axis.
        // Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        //Car.MoveRotation(Car.rotation * turnRotation);

    }
    private void Horn()
    {
        
            //Debug.Log("coucou");
        //A_car.pitch = startingPitch + Random.Range(-0.1f,0.1f);
        A_car.pitch = pitch;
            Harmony.Play(A_car);
            
    }
    
     void OnTriggerEnter(Collider boost)
    {
        if (boost.gameObject.tag == "BOOST")
        {
            boostOn = true;
        }
        if(boost.gameObject.tag == "BUMPER")
        {
            BUMP = true;
        }
    }
    void OnTriggerExit(Collider boost)
    {
        if (boost.gameObject.tag == "BOOST")
        {
            boostOn = false;
        }

    }
    void OnCollisionEnter(Collision ground)
    {
        if (ground.gameObject.tag == "LD" && auSol == false)
        {
            sol = true;
            Debug.Log("OUIIIIIII");
        }
        else
        {
            sol = false;
        }
    }
    void OnCollisionStay(Collision ground)
    {
        if(ground.gameObject.tag=="LD"&& auSol==false)
        {
            sol = true;
            Debug.Log("OUIIIIIII");
        }
        else
        {
            sol = false;
        }
    }
    private void Run()
    {
        Car.velocity = (new Vector3(-m_TurnInputValue * Speed,0, m_MovementInputValue * Speed));
            }
    /*
    private bool fail()
    {
        
        if(((Physics.Raycast(transform.position, transform.up, distToGround + tolerance))|| 
            Physics.Raycast(transform.position, transform.right, distToGround + tolerance)||
            Physics.Raycast(transform.position, -transform.right, distToGround + tolerance)||
            Physics.Raycast(transform.position, transform.forward, 4)||
            Physics.Raycast(transform.position, -transform.forward, distToGround + tolerance))
            && Car.transform.position.y<12)
        {
            Debug.Log("par terre");
            return true;
        }
        
        
        if(ground.gameObject.tag == "LD")
        {
            Debug.Log("oui");
            return true;
        }
        else
        {
            return false;
        }
        
    }
    */
    private void IsGrounded()
    {
        
        if(Physics.Raycast(transform.position, -transform.up, distToGround+tolerance))
        {


            auSol = true;
            return;
        }
        else
        {
            auSol = false;
        }
    }
    private void BarrelRoll()
    {
       
        //float _turn = Input.GetAxis("Horizontal");
        if (auSol)
        {
            Car.AddForce(transform.up * 1.5f, ForceMode.VelocityChange);

            StartCoroutine(getDown());
        }
        Car.AddRelativeTorque(new Vector3(1, 0, 0) ,ForceMode.VelocityChange );
        
    }
    private void Jump()
    {
        if(auSol)
        {
            //Debug.Log("hoi");
            Car.AddForce(transform.up*1.5f, ForceMode.VelocityChange);
            
            StartCoroutine(getDown());
        }
       
        
            Car.AddRelativeTorque(new Vector3(0, 0, 30), ForceMode.VelocityChange);
        BUMP = false;
    }
    IEnumerator getDown()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        Car.AddRelativeForce(new Vector3(0,-2,0),ForceMode.Impulse) ;
        //Debug.Log("stop");
    }
/*
private void GetUp()
{
  // Car.velocity=new Vector3 (0,1,0);
   // Quaternion _angle = Car.transform.rotation;
    Vector3 _good = new Vector3(0, Car.transform.rotation.y,0);
    Vector3 _bad = new Vector3 (Car.transform.rotation.x, Car.transform.rotation.y,Car.transform.rotation.z);

    if((_good.magnitude-_bad.magnitude<0.01 && _good.magnitude - _bad.magnitude>0)|| (_good.magnitude - _bad.magnitude<0 && _good.magnitude - _bad.magnitude>-0.01))
    {
        Car.velocity = new Vector3(Car.velocity.x, -2, Car.velocity.z);
    }

    Vector3 x = Vector3.Cross(_bad.normalized, _good.normalized);
    float theta = Mathf.Asin(x.magnitude);
    Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
    Quaternion q = transform.rotation * Car.inertiaTensorRotation;
    Vector3 T = q * Vector3.Scale(Car.inertiaTensor, (Quaternion.Inverse(q) * w));
    Car.AddTorque(T, ForceMode.Impulse);


}
*/
IEnumerator inactif()
    {
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("1");
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("2");
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("3");
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("deconnecte toi");
    }
    /*
    IEnumerator flip();
    {

    }
    */
    /*
    IEnumerator getUp()
    {
        
        yield return new WaitForSecondsRealtime(0.5f);
        Vector3 _good = new Vector3(0, Car.transform.rotation.y, 0);
        Vector3 _bad = new Vector3(Car.transform.rotation.x, Car.transform.rotation.y, Car.transform.rotation.z);



        Vector3 x = Vector3.Cross(_bad.normalized, _good.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
        Quaternion q = transform.rotation * Car.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(Car.inertiaTensor, (Quaternion.Inverse(q) * w));
        Car.AddTorque(T / 50, ForceMode.VelocityChange);
        yield return new WaitForSecondsRealtime(0.5f);
    }
    */
    
}




// Quaternion turnRotation = Quaternion.Euler(0f, _turn, 0f);
//Car.transform.Rotate(transform.rotation *turnRotation);
/* transform.Rotate(new Vector3(0, _turn * TurnSpeed*Time.deltaTime, 0));

 Quaternion rotation = Quaternion.Euler(0, _turn, 0);
 Vector3 direction = rotation * (new Vector3(0, 0, _go*Speed*Time.deltaTime));

 Car.AddRelativeForce(direction);
 */



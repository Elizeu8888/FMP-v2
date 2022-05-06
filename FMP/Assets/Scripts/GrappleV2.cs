using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrappleV2 : MonoBehaviour
{
    public LineRenderer lr1,lr2;
    Vector3 grapplePoint;
    Vector3 grappleAimPoint;
    public LayerMask whatsIsGrappleable;
    public LayerMask notGrappleable;
    public Transform camera,camLeft,camRight, player;
    public float maxDistance;
    SpringJoint joint;
    public Rig playerRig;
    //public Animator anim;
    public GameObject grapplepoint, grappleTip, grappleTip2;
    public Rigidbody rb;

    public float swing = 0.8f, pull = 0.05f;

    public float aimSize = 10f, aimSideSize = 2f, aimBlock = 3f;

    public bool isgrappling = false, isgrappling2 = false;

    public Animator anim;
    bool grappleMode = false, canGrapple = false;
    public float pullSpeed, upSpeed, pullCloseDistance;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            grappleMode = !grappleMode;
        }

        if(isgrappling == true || isgrappling2 == true)
        {
            anim.SetBool("Grappling", true);
        }
        else
        {
            anim.SetBool("Grappling", false);
        }


        if (grappleMode == true)
        {

            grappleTip2.SetActive(true);
            grappleTip.SetActive(true);


            if (Input.GetMouseButtonDown(1) && isgrappling2 == false)
            {
                StartGrapple(pull);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                StopGrapple();
                //playerRig.weight = 0;
                isgrappling = false;

            }
            if (Input.GetMouseButton(0) && isgrappling == true)
            {
                float distancefrompoint = Vector3.Distance(player.position, grapplePoint);

                if(distancefrompoint > pullCloseDistance)
                {
                    rb.AddForce((grapplePoint - transform.position).normalized * pullSpeed * Time.deltaTime, ForceMode.Impulse);
                    rb.AddForce(transform.up * upSpeed * Time.deltaTime, ForceMode.Impulse);
                }
                else
                {
                    StopGrapple();
                    //playerRig.weight = 0;
                    isgrappling = false;
                    rb.AddForce((grapplePoint - transform.position).normalized * pullSpeed * Time.deltaTime, ForceMode.Impulse);
                    rb.AddForce(transform.up * upSpeed * Time.deltaTime, ForceMode.Impulse);
                }
                

            }



            if (Physics.SphereCast(camera.position, aimSize, camera.forward, out RaycastHit aimhit, maxDistance, whatsIsGrappleable))
            {
                if (aimhit.transform.gameObject.layer == LayerMask.NameToLayer("grapplable"))
                {
                    grappleAimPoint = aimhit.point;
                    grapplepoint.SetActive(true);
                    grapplepoint.transform.position = grappleAimPoint;
                    canGrapple = true;
                }
                else
                {
                    grapplepoint.SetActive(false);
                    canGrapple = false;
                }

            }
            else
            {
                if (Physics.SphereCast(camRight.position, aimSideSize, camRight.forward, out RaycastHit aimside, maxDistance, whatsIsGrappleable))
                {
                    if (aimside.transform.gameObject.layer == LayerMask.NameToLayer("grapplable"))
                    {
                        grappleAimPoint = aimside.point;
                        grapplepoint.SetActive(true);
                        grapplepoint.transform.position = grappleAimPoint;
                        canGrapple = true;
                    }
                    else
                    {
                        grapplepoint.SetActive(false);
                        canGrapple = false;
                    }

                }
                else
                {

                    if (Physics.SphereCast(camLeft.position, aimSideSize, camLeft.forward, out RaycastHit aimsideL, maxDistance, whatsIsGrappleable))
                    {
                        if (aimsideL.transform.gameObject.layer == LayerMask.NameToLayer("grapplable"))
                        {
                            grappleAimPoint = aimsideL.point;
                            grapplepoint.SetActive(true);
                            grapplepoint.transform.position = grappleAimPoint;
                            canGrapple = true;
                        }
                        else
                        {
                            grapplepoint.SetActive(false);
                            canGrapple = false;
                        }


                    }
                    else
                    {
                        grapplepoint.SetActive(false);
                        canGrapple = false;
                    }
                }


            }
            
        }
        else
        {
            grappleTip2.SetActive(false);
            grappleTip.SetActive(false);
            StopGrapple();

        }



    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple(float maxdist)
    {
        RaycastHit hit;
        if(canGrapple == true)
        {
            if (Physics.SphereCast(camera.position, aimSize, camera.forward, out hit, maxDistance, whatsIsGrappleable))
            {

                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distancefrompoint = Vector3.Distance(player.position, grapplePoint);

                joint.maxDistance = distancefrompoint * maxdist;
                joint.minDistance = distancefrompoint * 0.05f;

                joint.spring = 5f;
                joint.damper = 2f;
                joint.massScale = 15f;
                rb.angularDrag = 0f;

                lr1.positionCount = 2;
                lr2.positionCount = 2;

                isgrappling = true;

                //playerRig.weight = 1;
            }
            else
            {
                if (Physics.SphereCast(camRight.position, aimSideSize, camRight.forward, out RaycastHit hitside, maxDistance, whatsIsGrappleable))
                {

                    grapplePoint = hitside.point;
                    joint = player.gameObject.AddComponent<SpringJoint>();
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = grapplePoint;

                    float distancefrompoint = Vector3.Distance(player.position, grapplePoint);
                    joint.maxDistance = distancefrompoint * maxdist;
                    joint.minDistance = distancefrompoint * 0.05f;

                   




                    joint.spring = 5f;
                    joint.damper = 2f;
                    joint.massScale = 15f;
                    rb.angularDrag = 0f;

                    lr1.positionCount = 2;
                    lr2.positionCount = 2;

                    isgrappling = true;
                }
                else
                {
                    if (Physics.SphereCast(camLeft.position, aimSideSize, camLeft.forward, out RaycastHit hitsideL, maxDistance, whatsIsGrappleable))
                    {

                        grapplePoint = hitsideL.point;
                        joint = player.gameObject.AddComponent<SpringJoint>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.connectedAnchor = grapplePoint;

                        float distancefrompoint = Vector3.Distance(player.position, grapplePoint);
                        joint.maxDistance = distancefrompoint * maxdist;
                        joint.minDistance = distancefrompoint * 0.05f;

                        joint.spring = 5f;
                        joint.damper = 2f;
                        joint.massScale = 15f;
                        rb.angularDrag = 0f;

                        lr1.positionCount = 2;
                        lr2.positionCount = 2;
                        isgrappling = true;

                    }
                    else
                    {
                        isgrappling = false;
                    }
                }
            }
        }

        
    }
    void DoubleGrappleStart()
    {
        if(Physics.SphereCast(camera.transform.position,aimSize,camera.transform.forward, out RaycastHit raycastHit,maxDistance,whatsIsGrappleable))
        {

        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr1.SetPosition(0, grappleTip.transform.position);
        lr1.SetPosition(1, grapplePoint);
        lr2.SetPosition(0, grappleTip2.transform.position);
        lr2.SetPosition(1, grapplePoint);
    }



    void StopGrapple()
    {
        lr1.positionCount = 0;
        lr2.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }


}
